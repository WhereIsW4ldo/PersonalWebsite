using Login.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Login;

public static class ServiceExtension
{
    public static void ConfigureLoginServices(this IServiceCollection services)
    {
        services.AddTransient<LoginService>();
        services.AddTransient<TokenService>();
    }
}