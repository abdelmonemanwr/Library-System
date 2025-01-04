using Library.Management.System.API.Models;
using Library.Management.System.API.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library.Management.System.API.Services.Implementations
{
    public class TokenService : ITokenService
    {
        public IConfiguration _configuration { get; }

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager, bool RememberMe)
        {
            // Registered & Private Claims
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var userRoles = await userManager.GetRolesAsync(user);
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            string? secretKey = _configuration["Jwt:SecretKey"];
            if (secretKey is null)
            {
                throw new ArgumentNullException("Secret Key doesn't exist");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            double days = 0;
            double.TryParse(_configuration["Jwt:AccessTokenDurationInDays"], out days);

            if (RememberMe)
            {
                days += 3;
            }

            var JwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(days),
                claims: authClaims,
                signingCredentials: signingCredentials // Signature
            );

            return new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);
        }
    }
}