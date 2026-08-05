using Scalar.AspNetCore;

namespace Curriculum.Api.Configurations;

public static class OpenApiConfiguration
{
    public static void ConfigureOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi();
    }
    
    public static void ConfigureOpenApi(
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
    }
}