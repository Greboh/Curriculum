using Curriculum.Infrastructure.Persistence;
using FluentAssertions;
using Xunit;

namespace Curriculum.UnitTests.Persistence;

public class CurriculumDataTests
{
    private readonly CurriculumData _uut = new();

    [Fact]
    public void DeleteSkill_ById_SkillExists_ShouldRemoveAndReturnSkill()
    {
        // Arrange
        var existing = _uut.Skills[0];
        var countBefore = _uut.Skills.Count;

        // Act
        var deleted = _uut.DeleteSkill(existing.Id, null);

        // Assert
        deleted
            .Should()
            .BeEquivalentTo(existing);

        _uut.Skills
            .Should()
            .HaveCount(countBefore - 1);

        _uut.Skills
            .Should()
            .NotContain(x => x.Id == existing.Id);
    }

    [Fact]
    public void DeleteSkill_ById_SkillDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var missingId = Guid.CreateVersion7();
        var countBefore = _uut.Skills.Count;

        // Act
        var deleted = _uut.DeleteSkill(missingId, null);

        // Assert
        deleted
            .Should()
            .BeNull();

        _uut.Skills
            .Should()
            .HaveCount(countBefore);
    }

    [Fact]
    public void DeleteSkill_ByName_SkillExists_ShouldRemoveAndReturnSkill()
    {
        // Arrange
        var existing = _uut.Skills[0];
        var countBefore = _uut.Skills.Count;

        // Act
        var deleted = _uut.DeleteSkill(null, existing.Name);

        // Assert
        deleted
            .Should()
            .BeEquivalentTo(existing);

        _uut.Skills
            .Should()
            .HaveCount(countBefore - 1);

        _uut.Skills
            .Should()
            .NotContain(x => x.Name == existing.Name);
    }

    [Fact]
    public void DeleteSkill_ByName_TrimsName_ShouldRemoveAndReturnSkill()
    {
        // Arrange
        var existing = _uut.Skills[0];
        var countBefore = _uut.Skills.Count;

        // Act
        var deleted = _uut.DeleteSkill(null, $"  {existing.Name}  ");

        // Assert
        deleted
            .Should()
            .BeEquivalentTo(existing);

        _uut.Skills
            .Should()
            .HaveCount(countBefore - 1);
    }

    [Fact]
    public void DeleteSkill_ByName_SkillDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var countBefore = _uut.Skills.Count;

        // Act
        var deleted = _uut.DeleteSkill(null, "Missing");

        // Assert
        deleted
            .Should()
            .BeNull();

        _uut.Skills
            .Should()
            .HaveCount(countBefore);
    }

    [Fact]
    public void DeleteSkill_ById_PrefersIdOverName_ShouldRemoveById()
    {
        // Arrange
        var existing = _uut.Skills[0];
        var otherName = _uut.Skills[1].Name;

        // Act
        var deleted = _uut.DeleteSkill(existing.Id, otherName);

        // Assert
        deleted
            .Should()
            .BeEquivalentTo(existing);

        _uut.Skills
            .Should()
            .Contain(x => x.Name == otherName);

        _uut.Skills
            .Should()
            .NotContain(x => x.Id == existing.Id);
    }
}