
using Curriculum.Api.Configurations;

namespace Curriculum.Api.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApi(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddProblemDetails();

        services.ConfigureGraphQL();
        services.ConfigureOpenApi();
        services.ConfigureVersioning();
        
        return services;
    }
}