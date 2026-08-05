using Scalar.AspNetCore;

namespace Curriculum.Api.Configurations;

public static class OpenApiConfiguration
{
    public static IServiceCollection ConfigureOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi();

        return services;
    }
    
    public static WebApplication ConfigureOpenApi(
        this WebApplication app,
        string title
    )
    {
        app
            .MapOpenApi()
            .WithDocumentPerVersion();
        
        app.MapScalarApiReference(opt =>
        {
            opt
                .WithTitle(title)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            
            foreach (var description in app.DescribeApiVersions())
            {
                opt.AddDocument(description.GroupName, description.GroupName);
            }
        });
        
        return app;
    }
}