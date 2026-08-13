
using Curriculum.Api.Configurations;
using Curriculum.Infrastructure.Configurations;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services;

namespace Curriculum.Api.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApi(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureHealthChecks();
        
        services
            .ConfigureGraphQL();
        
        services
            .AddServices()
            .AddPersistence(configuration);
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ISkillService, SkillService>();
        
        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services, 
        IConfiguration configuration
        )
    {
        services.ConfigureNpgsql(configuration, "curriculum");
        
        return services;
    }
}