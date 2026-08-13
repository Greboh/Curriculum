using Curriculum.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Curriculum.Infrastructure.Persistence;

public static class CurriculumSeeder
{
    extension(DbContext context)
    {
        public DbContext TrySeedCurriculumData()
        {
            return context
                .TrySeedCompanies(CancellationToken.None).GetAwaiter().GetResult()
                .TrySeedProjects(CancellationToken.None).GetAwaiter().GetResult()
                .TrySeedEducations(CancellationToken.None).GetAwaiter().GetResult()
                .TrySeedSkills(CancellationToken.None).GetAwaiter().GetResult();
        }
        
        public async Task<DbContext> TrySeedCurriculumDataAsync(CancellationToken ct)
        {
            await context.TrySeedCompanies(ct);
            await context.TrySeedProjects(ct);
            await context.TrySeedEducations(ct);
            await context.TrySeedSkills(ct);

            return context;
        }
        
        private async Task<DbContext> TrySeedCompanies(CancellationToken ct)
        {
            var hasAny = context
                .Set<Company>()
                .Any();

            if (hasAny)
            {
                
                return context;
            }
            
            context
                .Set<Company>()
                .AddRange(Companies);
                    
            await context.SaveChangesAsync(ct);

            return context;
        }
        private async Task<DbContext> TrySeedProjects(CancellationToken ct)
        {
            var hasAny = context
                .Set<Project>()
                .Any();

            if (hasAny)
            {
                return context;
            }
            
            context
                .Set<Project>()
                .AddRange(Projects);
                    
            await context.SaveChangesAsync(ct);

            return context;
        }
        
        private async Task<DbContext> TrySeedEducations(CancellationToken ct)
        {
            var hasAny = context
                .Set<Education>()
                .Any();

            if (hasAny)
            {
                return context;
            }
            
            context
                .Set<Education>()
                .AddRange(Educations);
                    
            await context.SaveChangesAsync(ct);

            return context;
        }
        
        private async Task<DbContext> TrySeedSkills(CancellationToken ct)
        {
            var hasAny = context
                .Set<Skill>()
                .Any();

            if (hasAny)
            {
                return context;
            }
            
            context
                .Set<Skill>()
                .AddRange(Skills);
                    
            await context.SaveChangesAsync(ct);

            return context;
        }
    }

    private static IReadOnlyList<Company> Companies { get; } =
    [
        new() { Id = Guid.CreateVersion7(), Name = "Tryg A/S" }
    ];
    
    private static IReadOnlyList<Project> Projects { get; } =
    [
        new() { Id = Guid.CreateVersion7(), Name = "Internal load-test tool" },
        new() { Id = Guid.CreateVersion7(), Name = "Straight-Through Processing (STP) claims" },
        new() { Id = Guid.CreateVersion7(), Name = "Modular GitHub Actions CI/CD" },
        new() { Id = Guid.CreateVersion7(), Name = "Microsoft Orleans production adoption" },
        new() { Id = Guid.CreateVersion7(), Name = "KPI platform data-ingestion / Orleans ingress" },
    ];
    
    private static IReadOnlyList<Education> Educations { get; } =
    [
        new()
        {
            Id = Guid.CreateVersion7(),
            Institution = "Erhvervsakademi Dania",
            Degree = "Professionsbachelor i Softwareudvikling",
            StartDate = new(2024, 1, 1),
            EndDate = new(2025, 12, 31),
        },
        new()
        {
            Id = Guid.CreateVersion7(),
            Institution = "Erhvervsakademi Dania",
            Degree = "Datamatiker",
            StartDate = new(2021, 1, 1),
            EndDate = new(2024, 12, 31),
        },
    ];

    private static readonly List<Skill> Skills =
    [
        new() { Id = Guid.CreateVersion7(), Name = "C#" },
        new() { Id = Guid.CreateVersion7(), Name = "ASP.NET Core" },
        new() { Id = Guid.CreateVersion7(), Name = "SQL" },
        new() { Id = Guid.CreateVersion7(), Name = "Microsoft Orleans" },
        new() { Id = Guid.CreateVersion7(), Name = "Dapr" },
        new() { Id = Guid.CreateVersion7(), Name = "MongoDB" },
        new() { Id = Guid.CreateVersion7(), Name = "Redis" },
        new() { Id = Guid.CreateVersion7(), Name = "RabbitMQ" },
        new() { Id = Guid.CreateVersion7(), Name = "Azure" },
        new() { Id = Guid.CreateVersion7(), Name = "Docker" },
        new() { Id = Guid.CreateVersion7(), Name = "Kubernetes" },
        new() { Id = Guid.CreateVersion7(), Name = "GitHub Actions" },
        new() { Id = Guid.CreateVersion7(), Name = "ArgoCD" },
        new() { Id = Guid.CreateVersion7(), Name = "Prompt Engineering / LLM integration" },
    ];
}