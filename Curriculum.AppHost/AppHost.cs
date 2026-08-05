
var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Curriculum_Api>("Api")
    .WithUrlForEndpoint("http", url =>
    {
        url.DisplayText = "Scalar";
        url.Url = "/scalar";
    })
    .WithUrl("http://localhost:5076/graphql", "GraphQL")
    .WithUrl("http://localhost:5076/ui/graphiql", "Graphiql");

builder
    .Build()
    .Run();
