using GraphQL;

namespace Curriculum.Api.Configurations;

public static class GraphQLConfiguration
{
    public static IServiceCollection ConfigureGraphQL(this IServiceCollection services)
    {
        services.AddGraphQL(x => x.AddSystemTextJson());
        
        return services;
    }

    public static WebApplication ConfigureGraphQL(this WebApplication app)
    {
        app.UseGraphQL();
        app.UseGraphQLGraphiQL();

        return app;
    }
}