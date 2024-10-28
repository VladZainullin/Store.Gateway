using Ocelot.DependencyInjection;

namespace Web;

internal static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddOcelot();
        
        services.AddHsts(static configureOptions =>
        {
            configureOptions.Preload = true;
            configureOptions.IncludeSubDomains = true;
            configureOptions.MaxAge = TimeSpan.FromDays(60);
        });
        
        services.AddHttpsRedirection(static options =>
        {
            options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            options.HttpsPort = 443;
        });

        services.AddHealthChecks();
        
        return services;
    }
}