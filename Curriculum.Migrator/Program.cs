using Curriculum.Infrastructure.Configurations;
using Curriculum.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.ConfigureNpgsql(
    builder.Configuration,
    builder.Configuration["DatabaseName"] 
        ?? throw new InvalidOperationException("Database name not found.")
);

using var host = builder.Build();
await using var scope = host.Services.CreateAsyncScope();

var db = scope.ServiceProvider.GetRequiredService<CurriculumContext>();

Console.WriteLine("Applying EF migrations...");
await db.Database.MigrateAsync();
Console.WriteLine("Done.");

return 0;