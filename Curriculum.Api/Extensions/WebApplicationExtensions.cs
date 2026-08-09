using Curriculum.Api.Configurations;

namespace Curriculum.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApi(
        this WebApplication app,
        IConfiguration configuration
    )
    {
        app.UseRouting();

        app.ConfigureHealthChecks();
        
        app.ConfigureGraphQL();
        
        return app;
    }
}
