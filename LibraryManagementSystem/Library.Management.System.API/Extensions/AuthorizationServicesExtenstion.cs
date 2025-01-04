using Library.Management.System.API.Models;

namespace Library.Management.System.API.Extensions
{
    public static class AuthorizationServicesExtenstion
    {
        public static IServiceCollection AddAuthorizationServices(this IServiceCollection Services)
        {
            Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireAssertion(context =>
                    {
                        if (!context.User.IsInRole("Admin"))
                        {
                            context.Fail();
                            return false;
                        }
                        return true;
                    }));
            });
            return Services;
        }
    }
}
