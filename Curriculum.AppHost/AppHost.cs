using Curriculum.AppHost.Configurations;

var builder = DistributedApplication.CreateBuilder(args);

var postgresDb = builder.ConfigurePostgres();

var migrator = builder.AddProject<Projects.Curriculum_Migrator>("migrator")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

builder.AddProject<Projects.Curriculum_Api>("Api")
    .WithReference(postgresDb)
    .WaitFor(migrator)
    .WithUrlForEndpoint("http", url =>
    {
        url.DisplayText = "Graphiql";
        url.Url = "/ui/graphiql";
    });

builder
    .Build()
    .Run();
