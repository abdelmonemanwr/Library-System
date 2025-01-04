using Library.Management.System.API.Models;

namespace Library.Management.System.API.Services.Abstractions
{
    public interface IBookService
    {
        // for Admins only
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);

        // for Admins and Users
        Task<Book?> GetBookByIdAsync(int id);
        Task<IEnumerable<Book?>> GetAllBooksAsync();
        Task<IEnumerable<Book?>> GetAllBooksAsync(string? title, string? author, string? genre, int page, int pageSize, string? sortBy, string? sortOrder);
    }
}