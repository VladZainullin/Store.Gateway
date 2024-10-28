using Ocelot.Middleware;
using Serilog;

namespace Web;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        await using var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile(
                path: "ocelot.json",
                optional: false,
                reloadOnChange: true);

            builder.Host.UseSerilog(logger);

            builder.Services.AddWebServices();

            await using var app = builder.Build();

            if (app.Environment.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseHealthChecks("/health");

            await app.UseOcelot();

            await app.RunAsync();
        }
        catch (Exception e)
        {
            logger.Fatal("Application not started. {error}", e);
        }
    }
}