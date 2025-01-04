using System.ComponentModel.DataAnnotations;

namespace Library.Management.System.API.DTOs
{
    public class BookDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public int PublishedYear { get; set; }
    }
}
