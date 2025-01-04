using Library.Management.System.API.Models;
using Library.Management.System.API.Services.Abstractions;
using Library.Management.System.API.Unit_of_Work;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Library.Management.System.API.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public BookService(IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task AddBookAsync(Book book)
        {
            _unitOfWork.Repository<Book>().Add(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _unitOfWork.Repository<Book>().Update(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _unitOfWork.Repository<Book>().GetByIdAsync(id);
            if (book == null) throw new KeyNotFoundException("Book not found");

            _unitOfWork.Repository<Book>().Delete(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id) 
            => await _unitOfWork.Repository<Book>().GetByIdAsync(id);

        public async Task<IEnumerable<Book?>> GetAllBooksAsync()
            => await _unitOfWork.Repository<Book>().GetAllAsync();

        public async Task<IEnumerable<Book?>> GetAllBooksAsync(string? title, string? author, string? genre, int page, int pageSize, string? sortBy, string? sortOrder)
        {
            ValidatePages(page, pageSize);

            var cacheKey = GenerateCacheKey(title, author, genre, page, pageSize, sortBy, sortOrder);

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Book>? books))
            {
                // short-circuiting to build the predicate criteria
                Expression<Func<Book, bool>> predicate = b =>
                    (string.IsNullOrEmpty(title) || b.Title.Contains(title)) &&
                    (string.IsNullOrEmpty(author) || b.Author.Contains(author)) &&
                    (string.IsNullOrEmpty(genre) || b.Genre.Contains(genre));

                // Apply sorting
                IQueryable<Book> query = _unitOfWork.Repository<Book>().Find(predicate);

                switch (sortBy?.ToLower())
                {
                    case "author":
                        query = (sortOrder?.ToLower() == "desc" ?
                                    query.OrderByDescending(b => b.Author) :
                                    query.OrderBy(b => b.Author)
                                );
                        break;

                    case "genre":
                        query = (sortOrder?.ToLower() == "desc" ?
                                    query.OrderByDescending(b => b.Genre) :
                                    query.OrderBy(b => b.Genre)
                                );
                        break;

                    default: // Default sort by title
                        query = (sortOrder?.ToLower() == "desc" ?
                                    query.OrderByDescending(b => b.Title) :
                                    query.OrderBy(b => b.Title)
                                );
                        break;
                }

                // Paginate
                books = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, books, cacheOptions);
            }

            return books!;
        }

        #region Helper Methods
        private static string GenerateCacheKey(string? title, string? author, string? genre, int page, int pageSize, string? sortBy, string? sortOrder)
        {
            return $"Books_{title ?? "null"}_{author ?? "null"}_{genre ?? "null"}_{page}_{pageSize}_{sortBy ?? "null"}_{sortOrder ?? "asc"}";
        }


        private static void ValidatePages(int page, int pageSize)
        {
            if (page <= 0)
                throw new ArgumentException("Page must be greater than zero.");

            if (pageSize <= 0)
                throw new ArgumentException("PageSize must be greater than zero.");
        }
        #endregion
    }
}
