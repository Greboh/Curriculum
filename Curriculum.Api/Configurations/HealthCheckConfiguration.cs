using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Curriculum.Api.Configurations;

public static class HealthCheckConfiguration
{
    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
        
        return services;
    }

    public static WebApplication ConfigureHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/alive", new()
        {
            Predicate = r => r.Tags.Contains("live")
        });
        
        return app;
    }
}