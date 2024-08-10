using FinalCampus.Repositories.Auth;

namespace FinalCampus.DiController;

public static class RepoDependencyInjection
{
    public static IServiceCollection RepositoryServices(this IServiceCollection Services)
    {
        Services.AddScoped<ITokenService, TokenService>();

        return Services;
    }
}