using Library.Management.System.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Library.Management.System.API.Services.Abstractions
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager, bool RememberMe);
    }
}
