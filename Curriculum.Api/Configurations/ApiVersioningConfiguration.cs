namespace Curriculum.Api.Configurations;

public static class ApiVersioningConfiguration
{
    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new(
                    majorVersion: 1,
                    minorVersion: 0
                );
            })
            .AddMvc()
            .AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            })
            .AddOpenApi();
    }
}