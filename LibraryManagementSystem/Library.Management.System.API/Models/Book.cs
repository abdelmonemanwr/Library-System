namespace Library.Management.System.API.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Author { get; set; }
        
        public string Genre { get; set; }
        
        public int PublishedYear { get; set; }
        
        // I assume that the book is available by default
        public bool IsAvailable { get; set; } = true;
    }
}
