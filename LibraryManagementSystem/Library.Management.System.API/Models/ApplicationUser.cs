using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Library.Management.System.API.Models
{
    public enum Gender: byte { 
        Male = 0, 
        Female = 1 
    }

    public class ApplicationUser: IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? ProfileImage { get; set; }

        public string? PhoneNumber { get; set; }

        public Gender? Gender { get; set; }

        public string Role { get; set; }
    }
}
