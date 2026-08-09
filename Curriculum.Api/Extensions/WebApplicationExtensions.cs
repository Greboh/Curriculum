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
        app.UseRouting();
        
        app.MapDefaultEndpoints();
        
        app.ConfigureGraphQL();
        
        return app;
    }
}
