namespace Curriculum.AppHost.Configurations;

public static class PostgresConfiguration
{
    public static IResourceBuilder<PostgresServerResource> ConfigurePostgres(this IDistributedApplicationBuilder builder)
    {
        var username = builder.AddParameter("Npgsql-Username", "admin");
        var password = builder.AddParameter("Npgsql-Password", "Test-1234", secret: true);

        return builder
            .AddPostgres("Npgsql", username, password, port: 5432)
            .WithLifetime(ContainerLifetime.Persistent)
            .WithDataVolume()
            .WithDbGate(configureContainer: resourceBuilder =>
            {
                resourceBuilder.WithHostPort(1234);
            });
    }
}