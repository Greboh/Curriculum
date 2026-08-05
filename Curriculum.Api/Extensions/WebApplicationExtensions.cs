using Curriculum.Api.Configurations;
using Curriculum.Api.Options;
using Curriculum.Infrastructure.Configurations;

namespace Curriculum.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApi(
        this WebApplication app,
        IConfiguration configuration
    )
    {
        var serviceOptions = ServiceOptions.Get(configuration);
        
        app.UseRouting();
        
        app.MapDefaultEndpoints();
        
        app.ConfigureOpenApi(serviceOptions.Name);
        app.ConfigureGraphQL();

        app.MapControllers();

        return app;
    }
}
