using Curriculum.Core.Entities;

namespace Curriculum.Tests.Shared;

public static class FakeCurriculumData
{
    public static IReadOnlyList<Company> Companies { get; } =
    [
        new()
        {
            Id = Guid.CreateVersion7(),
            Name = "Test Company 1"
        },
        new()
        {
            Id = Guid.CreateVersion7(),
            Name = "Test Company 2"
        }
    ];

    public static IReadOnlyList<Project> Projects { get; } =
    [
        new()
        {
            Id = Guid.CreateVersion7(),
            Name = "Test Project 1"
        },
        new()
        {
            Id = Guid.CreateVersion7(),
            Name = "Test Project 2"
        }
    ];

    public static IReadOnlyList<Education> Educations { get; } =
    [
        new()
        {
            Id = Guid.CreateVersion7(),
            Institution = "Test Institution 1",
            Degree = "Test Degree 1",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
        },
        new()
        {
            Id = Guid.CreateVersion7(),
            Institution = "Test Institution 2",
            Degree = "Test Degree 2",
            StartDate = new(2026, 1, 1),
            EndDate = new(2026, 12, 31),
        },
    ];

    public static IReadOnlyList<Skill> Skills { get; } =
    [
        new()
        {
            Id = Guid.CreateVersion7(),
            Name = "Test Skill 1"
        },
        new()
        {
            Id = Guid.CreateVersion7(),
            Name = "Test Skill 2"
        },
    ];
}