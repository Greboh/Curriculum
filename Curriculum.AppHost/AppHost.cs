
var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Curriculum_Api>("Api")
    .WithUrlForEndpoint("http", url =>
    {
        url.DisplayText = "Scalar";
        url.Url = "/scalar";
    });

builder
    .Build()
    .Run();
