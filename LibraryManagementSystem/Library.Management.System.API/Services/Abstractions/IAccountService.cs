using Library.Management.System.API.DTOs;

namespace Library.Management.System.API.Services.Abstractions
{
    public interface IAccountService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);
    }
}
