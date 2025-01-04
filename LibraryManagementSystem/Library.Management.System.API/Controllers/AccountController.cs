using AutoMapper;
using Library.Management.System.API.DTOs;
using Library.Management.System.API.Models;
using Library.Management.System.API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using System.Text;

namespace Library.Management.System.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;
        private readonly IAccountService accountService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(
            IMapper mapper,
            ITokenService tokenService,
            IAccountService accountService,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager
        ) {
            this.mapper = mapper;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.signInManager = signInManager;
            this.accountService = accountService;
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="loginDTO">Login credentials (email and password).</param>
        /// <returns>Returns an authentication token if successful or an error message if the login fails.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDTO)
        {
            var auth = await accountService.LoginAsync(loginDTO);
            return auth.isSuccess ? Ok(auth) : BadRequest(auth);
        }

        /// <summary>
        /// Check if an email is already registered.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>Returns true if the email already exists, otherwise false.</returns>
        [HttpGet("check-existing-email")]
        public async Task<ActionResult<bool>> CheckExistingEmail(string email)
        {
            return Ok(await userManager.FindByEmailAsync(email) != null);
        }

        /// <summary>
        /// User Registration
        /// </summary>
        /// <param name="registerDTO">Registration data including email, password, confirm password, and role.</param>
        /// <returns>Returns a success message and user details if registration is successful, or an error message if validation fails.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO registerDTO)
        {
            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                return BadRequest(new { isSuccess = false, Message = "Passwords do not match" });
            }

            var emailExists = await CheckExistingEmail(registerDTO.Email!);
            if (emailExists.Value)
            {
                return BadRequest(new { isSuccess = false, Message = "Email already exists" });
            }

            var appUser = mapper.Map<ApplicationUser>(registerDTO);

            var role = string.IsNullOrEmpty(registerDTO.Role) ? "User" : registerDTO.Role;

            var result = await userManager.CreateAsync(appUser, registerDTO.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return BadRequest(new { isSuccess = false, Message = errors });
            }

            var roleResult = await userManager.AddToRolesAsync(appUser, new List<string> { role });
            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(appUser);
                var roleErrors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                return BadRequest(new { isSuccess = false, Message = $"Failed to assign roles: {roleErrors}" });
            }

            return Ok(new
            {
                isSuccess = true,
                Message = $"{appUser.FirstName} has registered successfully",
            });
        }

        /// <summary>
        /// User Logout
        /// </summary>
        /// <returns>Returns a success message indicating the user has been logged out.</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<AuthResponseDTO>> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new
            {
                isSuccess = true,
                Message = "Logout Successful"
            });
        }
    }
}
