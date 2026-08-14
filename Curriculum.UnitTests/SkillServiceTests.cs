using Curriculum.Core.Entities;
using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using Curriculum.UnitTests.Setup;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Curriculum.UnitTests;

public class SkillServiceTests : TestBase
{
    private readonly SkillService _uut;

    public SkillServiceTests()
    {
        _uut = new(Context);
    }

    [Fact]
    public async Task Create_NameIsValid_ShouldCreateAndReturnSkill()
    {
        // Arrange
        const string name = "GraphQL";
        var expectation = new Skill
        {
            Id = Guid.Empty,
            Name = name
        };

        // Act
        var result = await _uut.Create(name);

        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation, opt => opt.Excluding(x => x.Id));

        result.Value!.Id
            .Should()
            .NotBeEmpty();

        (await Context.Skills.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name))
            .Should()
            .NotBeNull();
    }

    [Fact]
    public async Task Create_NameIsNullOrEmpty_ShouldReturnValidationError()
    {
        // Arrange
        const string name = "";
        var expectation = new SkillValidationError(
            name,
            new Dictionary<string, object>
            {
                { "Name", "Is Null or Empty" }
            }
        );

        // Act
        var result = await _uut.Create(name);

        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);

        Context.Skills
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Delete_ByName_SkillExists_ShouldReturnTrueAndRemoveSkill()
    {
        // Arrange
        var skill = new Skill
        {
            Id = Guid.CreateVersion7(),
            Name = "C#"
        };
        Context.Skills.Add(skill);
        await Context.SaveChangesAsync();

        // Act
        var deleted = await _uut.Delete(null, skill.Name);

        // Assert
        deleted.Should().BeTrue();
        Context.Skills.Should().NotContain(x => x.Id == skill.Id);
    }

    [Fact]
    public async Task Delete_ById_SkillExists_ShouldReturnTrueAndRemoveSkill()
    {
        // Arrange
        var skill = new Skill
        {
            Id = Guid.CreateVersion7(),
            Name = "C#"
        };
        Context.Skills.Add(skill);
        await Context.SaveChangesAsync();

        // Act
        var deleted = await _uut.Delete(skill.Id, null);

        // Assert
        deleted.Should().BeTrue();
        Context.Skills.Should().NotContain(x => x.Id == skill.Id);
    }

    [Fact]
    public async Task Delete_ByName_SkillDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        const string name = "Missing";

        // Act
        var deleted = await _uut.Delete(null, name);

        // Assert
        deleted.Should().BeFalse();
    }

    [Fact]
    public async Task GetAll_DataContainsSkills_ShouldReturnAllSkills()
    {
        // Arrange
        Context.Skills.AddRange(FakeCurriculumData.Skills);
        await Context.SaveChangesAsync();
        var expectation = FakeCurriculumData.Skills;

        // Act
        var result = await _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task GetAll_DataDoesNotContainSkills_ShouldReturnEmptyList()
    {
        // Act
        var result = await _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Get_ById_DataContainsSkill_ShouldReturnSkill()
    {
        // Arrange
        Context.Skills.AddRange(FakeCurriculumData.Skills);
        await Context.SaveChangesAsync();
        var expectation = FakeCurriculumData.Skills[0];

        // Act
        var result = await _uut.Get(expectation.Id, null);

        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Get_ById_DataDoesNotContainSkill_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Skills[0].Id;
        var expectation = new SkillNotFoundError(id);

        // Act
        var result = await _uut.Get(id, null);

        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Get_ByName_DataContainsSkill_ShouldReturnSkill()
    {
        // Arrange
        Context.Skills.AddRange(FakeCurriculumData.Skills);
        await Context.SaveChangesAsync();
        var expectation = FakeCurriculumData.Skills[0];

        // Act
        var result = await _uut.Get(null, expectation.Name);

        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Get_ByName_DataDoesNotContainSkill_ShouldReturnNotFoundError()
    {
        // Arrange
        const string name = "Missing";
        Context.Skills.AddRange(FakeCurriculumData.Skills);
        await Context.SaveChangesAsync();
        var expectation = new SkillNotFoundError(name);

        // Act
        var result = await _uut.Get(null, name);

        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}