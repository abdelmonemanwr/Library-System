namespace Library.Management.System.API.Models
{
    public class BorrowedBook
    {
        public int Id { get; set; }

        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string UserId { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
