using Library.Management.System.API.Models;

namespace Library.Management.System.API.Services.Abstractions
{
    public interface IBorrowingService
    {
        Task BorrowBookAsync(int bookId, string userId);
        Task<IReadOnlyList<BorrowedBook>> GetBorrowedBooksAsync(string userId);
        Task ReturnBookAsync(int bookId, string userId);
    }
}
