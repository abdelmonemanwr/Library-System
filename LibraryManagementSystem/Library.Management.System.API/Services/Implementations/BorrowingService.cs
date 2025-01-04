using Library.Management.System.API.Models;
using Library.Management.System.API.Services.Abstractions;
using Library.Management.System.API.Unit_of_Work;

namespace Library.Management.System.API.Services.Implementations
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task BorrowBookAsync(int bookId, string userId)
        {
            try
            {
                if(string.IsNullOrEmpty(userId))
                    throw new InvalidOperationException("User ID is required.");

                var book = await _unitOfWork.Repository<Book>().GetByIdAsync(bookId);

                if (book == null)
                    throw new InvalidOperationException("Book doesn't exist");

                if (!book.IsAvailable)
                    throw new InvalidOperationException("Book is unavailable.");

                // Check if the book was borrowed
                var borrowedBooks = await _unitOfWork.Repository<BorrowedBook>()
                    .FindAsync(bb => bb.BookId == bookId && bb.UserId == userId && bb.ReturnDate == null);

                if (borrowedBooks.Any())
                    throw new InvalidOperationException("You have already borrowed this book.");

                // Mark the book as borrowed
                book.IsAvailable = false;
                _unitOfWork.Repository<Book>().Update(book);

                var borrowedBook = new BorrowedBook
                {
                    BookId = bookId,
                    UserId = userId,
                    BorrowedDate = DateTime.UtcNow
                };

                _unitOfWork.Repository<BorrowedBook>().Add(borrowedBook);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<IReadOnlyList<BorrowedBook>> GetBorrowedBooksAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new InvalidOperationException("User ID is required.");

            return await _unitOfWork.Repository<BorrowedBook>()
                .FindAsync(bb => bb.UserId == userId && bb.ReturnDate == null);
        }


        public async Task ReturnBookAsync(int bookId, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new InvalidOperationException("User ID is required.");

                // get the borrowed book record
                var borrowedBook = (await _unitOfWork.Repository<BorrowedBook>()
                    .FindAsync(bb => bb.BookId == bookId && bb.UserId == userId && bb.ReturnDate == null))
                    .FirstOrDefault();

                if (borrowedBook == null)
                    throw new InvalidOperationException("No active borrow record found for this book.");

                // if the borrowed book is overdue
                if ((DateTime.UtcNow - borrowedBook.BorrowedDate).TotalDays > 14)
                {
                    throw new InvalidOperationException("The book is overdue for return.");
                }

                // Mark the book as returned
                borrowedBook.ReturnDate = DateTime.UtcNow;
                _unitOfWork.Repository<BorrowedBook>().Update(borrowedBook);

                var book = await _unitOfWork.Repository<Book>().GetByIdAsync(bookId);
                book.IsAvailable = true;
                _unitOfWork.Repository<Book>().Update(book);

                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
