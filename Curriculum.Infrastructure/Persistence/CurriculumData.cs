using Curriculum.Core.Entities;

namespace Curriculum.Infrastructure.Persistence;

public static class CurriculumData
{
    public static IReadOnlyList<Company> Companies { get; } =
    [
        new()
        {
            Id = Guid.CreateVersion7(),
            Name = "Tryg A/S"
        }
    ];
    
    public static IReadOnlyList<Project> Projects { get; } =
    [
        new() { Id = Guid.CreateVersion7(), Name = "Internal load-test tool" },
        new() { Id = Guid.CreateVersion7(), Name = "Straight-Through Processing (STP) claims" },
        new() { Id = Guid.CreateVersion7(), Name = "Modular GitHub Actions CI/CD" },
        new() { Id = Guid.CreateVersion7(), Name = "Microsoft Orleans production adoption" },
        new() { Id = Guid.CreateVersion7(), Name = "KPI platform data-ingestion / Orleans ingress" },
    ];
    
    public static IReadOnlyList<Education> Education { get; } =
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
    
    public static IReadOnlyList<Skill> Skills { get; } =
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