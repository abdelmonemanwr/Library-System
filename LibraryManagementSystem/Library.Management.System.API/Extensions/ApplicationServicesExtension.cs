using Library.Management.System.API.Helpers;
using Library.Management.System.API.Repositories;
using Library.Management.System.API.Services.Abstractions;
using Library.Management.System.API.Services.Implementations;
using Library.Management.System.API.Unit_of_Work;
using Microsoft.AspNetCore.Authorization;

namespace Library.Management.System.API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Services.AddScoped<IBookService, BookService>();

            Services.AddScoped<ITokenService, TokenService>();

            Services.AddScoped<IAccountService, AccountService>();

            Services.AddScoped<IBorrowingService, BorrowingService>();

            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationHandler>();

            return Services;
        }
    }
}
