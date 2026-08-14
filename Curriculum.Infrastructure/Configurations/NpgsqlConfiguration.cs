using Curriculum.Core.Entities;
using Curriculum.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Curriculum.Infrastructure.Configurations;

public static class NpgsqlConfiguration
{
    public static void ConfigureNpgsql(
        this IServiceCollection services, 
        IConfiguration configuration,
        string databaseName
        )
    {
        services.AddDbContext<CurriculumContext>(opt =>
        {
            var connectionString = configuration.GetConnectionString("Npgsql");

            opt.UseNpgsql($"{connectionString};Database={databaseName};");
            opt.UseSeeding((context, _) =>
            {
                context.TrySeedCurriculumData();
            })
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                await context.TrySeedCurriculumDataAsync(cancellationToken);
            })
            ;
        });
    }
}