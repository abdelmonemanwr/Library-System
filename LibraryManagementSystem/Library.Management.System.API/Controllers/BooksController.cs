using AutoMapper;
using Library.Management.System.API.DTOs;
using Library.Management.System.API.Models;
using Library.Management.System.API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Management.System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new book to the library.
        /// Only accessible by Admin users.
        /// </summary>
        /// <param name="bookDTO">The book details to be added.</param>
        /// <returns>Returns a success message and the added book's details.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBook(BookDTO bookDTO)
        {
            var book = _mapper.Map<Book>(bookDTO);
            await _bookService.AddBookAsync(book);
            return Ok(new { message = "Book added successfully", book });
        }


        /// <summary>
        /// Updates an existing book's details.
        /// Only accessible by Admin users.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="bookDTO">The new details of the book to update.</param>
        /// <returns>Returns a success message with the updated book details, or an error if validation fails.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(int id, BookUpdateDTO bookDTO)
        {
            if (id != bookDTO.Id)
            {
                return BadRequest(new { 
                    isSuccess = false, 
                    Message = "Please, enter a valid id." 
                });
            }

            var existingBook = await _bookService.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound(new { 
                    isSuccess = false, 
                    Message = "Book not found" 
                });
            }
            _mapper.Map(bookDTO, existingBook);

            await _bookService.UpdateBookAsync(existingBook);
            return Ok(new { isSuccess = true, Message = "Book updated successfully.", Book = existingBook });
        }

        /// <summary>
        /// Deletes a book from the library.
        /// Only accessible by Admin users.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>Returns a success message if the book is deleted, or an error if the book is not found.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var bookExists = await _bookService.GetBookByIdAsync(id);
            if (bookExists == null)
            {
                return NotFound("Book not found");
            }
            await _bookService.DeleteBookAsync(id);
            return Ok("Book deleted successfully");
        }

        /// <summary>
        /// Retrieves a list of books based on optional filters such as title, author, genre, pagination, and sorting.
        /// </summary>
        /// <param name="title">Optional filter for the book title.</param>
        /// <param name="author">Optional filter for the author's name.</param>
        /// <param name="genre">Optional filter for the genre of the book.</param>
        /// <param name="page">The page number for pagination (default is 1).</param>
        /// <param name="pageSize">The number of books per page (default is 5).</param>
        /// <param name="sortBy">Optional field to sort the books by (e.g., "title", "author").</param>
        /// <param name="sortOrder">Optional sorting order (asc for ascending, desc for descending; default is "asc").</param>
        /// <returns>Returns a paginated and sorted list of books matching the search criteria.</returns>
        [HttpGet]
        public async Task<IActionResult> GetBooks(string? title, string? author, string? genre, int page = 1, int pageSize = 5, string? sortBy = "title", string? sortOrder = "asc")
        {
            return Ok(await _bookService.GetAllBooksAsync(title, author, genre, page, pageSize, sortBy, sortOrder));
        }

    }
}