using Library.Management.System.API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Library.Management.System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BorrowingController : ControllerBase
    {
        private readonly IBorrowingService _borrowingService;

        public BorrowingController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }

        /// <summary>
        /// Endpoint to borrow a book.
        /// </summary>
        /// <param name="bookId">ID of the book to borrow.</param>
        /// <returns>Returns a success message if borrowing the book was done successfully</returns>
        [HttpPost("{bookId}")]
        [Authorize]
        public async Task<IActionResult> BorrowBook(int bookId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

                await _borrowingService.BorrowBookAsync(bookId, userId);

                return Ok(new { Message = "Book borrowed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        /// <summary>
        /// Endpoint to retrieve a list of books borrowed by the user who logged in.
        /// </summary>
        /// <returns>Returns a list of borrowed books with details like borrowed date, return date, user ID, and book ID.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBorrowedBooks()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                
                var borrowedBooks = await _borrowingService.GetBorrowedBooksAsync(userId);

                var result = borrowedBooks.Select(bb => new
                {
                    borrowedDate = bb.BorrowedDate,
                    returnDate = bb.BorrowedDate.AddDays(14),
                    userId = bb.UserId,
                    bookId = bb.BookId
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        /// <summary>
        /// Endpoint to return a borrowed book.
        /// </summary>
        /// <param name="bookId">ID of the book to return.</param>
        /// <returns>Returns a success message if the book was returned successfully.</returns>
        [HttpPost("return/{bookId}")]
        [Authorize]
        public async Task<IActionResult> ReturnBook(int bookId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

                await _borrowingService.ReturnBookAsync(bookId, userId);

                return Ok(new { Message = "Book returned successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
