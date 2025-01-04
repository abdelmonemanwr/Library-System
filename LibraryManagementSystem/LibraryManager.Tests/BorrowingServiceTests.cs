using Library.Management.System.API.Models;
using Library.Management.System.API.Services.Abstractions;
using Library.Management.System.API.Services.Implementations;
using Library.Management.System.API.Unit_of_Work;
using Moq;
using System.Linq.Expressions;


namespace Library.Management.System.API.Tests
{
    public class BorrowingServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IBorrowingService _borrowingService;

        public BorrowingServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _borrowingService = new BorrowingService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task BorrowBook_ShouldBorrowBookSuccessfully()
        {
            // Arrange
            var bookId = 5;
            var userId = "8827aa61-b06a-444f-aa38-8ab83e79df04";
            var book = new Book { Id = bookId, IsAvailable = true };

            _unitOfWorkMock.Setup(unit => unit.Repository<Book>().GetByIdAsync(bookId))
                .ReturnsAsync(book);

            _unitOfWorkMock.Setup(unit => unit.Repository<BorrowedBook>()
                .FindAsync(It.IsAny<Expression<Func<BorrowedBook, bool>>>()))
                .ReturnsAsync(new List<BorrowedBook>());

            // Act
            await _borrowingService.BorrowBookAsync(bookId, userId);

            // Assert
            _unitOfWorkMock.Verify(unit => unit.Repository<Book>().Update(It.Is<Book>(b => !b.IsAvailable)), Times.Once);
            _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ReturnBook_ShouldThrowIfOverdue()
        {
            // Arrange
            var bookId = 5;
            var userId = "8827aa61-b06a-444f-aa38-8ab83e79df04";   // Role: User

            var borrowedBook = new BorrowedBook
            {
                BookId = bookId,
                UserId = userId,
                BorrowedDate = DateTime.UtcNow.AddDays(-15) // Overdue
            };

            _unitOfWorkMock.Setup(unit => unit.Repository<BorrowedBook>()
                .FindAsync(It.IsAny<Expression<Func<BorrowedBook, bool>>>()))
                .ReturnsAsync(new List<BorrowedBook>());


            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _borrowingService.ReturnBookAsync(bookId, userId));
        }
    }
}
