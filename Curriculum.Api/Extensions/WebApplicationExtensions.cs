using Curriculum.Api.Configurations;
using Curriculum.Infrastructure.Configurations;

namespace Curriculum.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApi(
        this WebApplication app,
        IConfiguration configuration
    )
    {
        app.ConfigureSerilogRequestLogging();
        
        app.UseRouting();

        app.ConfigureHealthChecks();
        
        app.ConfigureGraphQL();
        
        return app;
    }
}
