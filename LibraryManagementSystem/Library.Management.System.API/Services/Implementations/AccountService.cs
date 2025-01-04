using AutoMapper;
using Library.Management.System.API.DTOs;
using Library.Management.System.API.Models;
using Library.Management.System.API.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Library.Management.System.API.Services.Implementations
{
    public class AccountService: IAccountService
    {
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager, IMapper mapper, ITokenService tokenService)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signManager = signManager;
            this.tokenService = tokenService;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return new AuthResponseDTO
                {
                    isSuccess = false,
                    Message = "Invalid credentials."
                };
            }

            var result = await signManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                return new AuthResponseDTO { 
                    isSuccess = false, 
                    Message = "Invalid Email or Password" 
                };
            }

            var token = await tokenService.GenerateTokenAsync(user, userManager, loginDTO.RememberMe);

            return new AuthResponseDTO
            {
                Token = token,
                isSuccess = true,
                Message = $"Welcome back {user.FirstName} {user.LastName} 😘"
            };
        }
    }
}
