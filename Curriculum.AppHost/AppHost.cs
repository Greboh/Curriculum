
var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Curriculum_Api>("Api")
    .WithUrlForEndpoint("http", url =>
    {
        url.DisplayText = "Graphiql";
        url.Url = "/ui/graphiql";
    });

builder
    .Build()
    .Run();
