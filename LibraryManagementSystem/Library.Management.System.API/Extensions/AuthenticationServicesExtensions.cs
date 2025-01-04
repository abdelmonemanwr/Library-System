using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Library.Management.System.API.Extensions
{
    public static class AuthenticationServicesExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection Services, IConfiguration builderConfiguration)
        {
            Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        
                        ValidateAudience = true,
                        
                        ValidateLifetime = true,
                        
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builderConfiguration["Jwt:Issuer"],
                        
                        ValidAudience = builderConfiguration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builderConfiguration["Jwt:SecretKey"]!))
                    };
                });
            return Services;
        }
    }
}