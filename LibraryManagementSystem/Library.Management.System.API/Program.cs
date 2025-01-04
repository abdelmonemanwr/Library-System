
using Library.Management.System.API.Extensions;
using Library.Management.System.API.Helpers;
using Library.Management.System.API.Models;
using Library.Management.System.API.Repositories;
using Library.Management.System.API.Services.Abstractions;
using Library.Management.System.API.Services.Implementations;
using Library.Management.System.API.Unit_of_Work;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Library.Management.System.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddDbContext<LibraryContext>(options => {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                );
            });

            builder.Services.AddMemoryCache();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // My Extension Methods
            builder.Services.AddSwaggerService();
            builder.Services.AddIdentityServices();
            builder.Services.AddApplicationServices();
            builder.Services.AddAuthorizationServices();
            builder.Services.AddAuthenticationServices(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
