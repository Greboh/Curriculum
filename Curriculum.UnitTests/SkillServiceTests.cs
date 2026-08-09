using Curriculum.Core.Entities;
using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using Curriculum.UnitTests.Setup;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Curriculum.UnitTests;

public class SkillServiceTests : TestBase
{
    private readonly SkillService _uut;

    public SkillServiceTests()
    {
        _uut = new(CurriculumDataMock);
    }

    [Fact]
    public void Create_NameIsValid_ShouldCreateAndReturnSkill()
    {
        // Arrange
        const string name = "GraphQL";
        CurriculumDataMock
            .CreateSkill(Arg.Any<Skill>())
            .Returns(x => x.Arg<Skill>());

        var expectation = new Skill
        {
            Id = Guid.Empty,
            Name = name
        };

        // Act
        var result = _uut.Create(name);

        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation, opt => opt.Excluding(x => x.Id));

        result.Value.Id
            .Should()
            .NotBeEmpty();

        CurriculumDataMock
            .Received(1)
            .CreateSkill(Arg.Is<Skill>(x => x.Name == name));
    }

    [Fact]
    public void Create_NameIsNullOrEmpty_ShouldReturnValidationError()
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
        var result = _uut.Create(name);

        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);

        CurriculumDataMock
            .DidNotReceive()
            .CreateSkill(Arg.Any<Skill>());
    }

    [Fact]
    public void Delete_SkillExists_ShouldReturnTrue()
    {
        // Arrange
        const string name = "C#";
        CurriculumDataMock
            .DeleteSkill(name)
            .Returns(true);

        // Act
        var result = _uut.Delete(name);

        // Assert
        result.Value
            .Should()
            .BeTrue();

        CurriculumDataMock
            .Received(1)
            .DeleteSkill(name);
    }

    [Fact]
    public void Delete_SkillDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        const string name = "Missing";
        CurriculumDataMock
            .DeleteSkill(name)
            .Returns(false);

        // Act
        var result = _uut.Delete(name);

        // Assert
        result.Value
            .Should()
            .BeFalse();
    }

    [Fact]
    public void Delete_NameIsNullOrEmpty_ShouldReturnValidationError()
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
        var result = _uut.Delete(name);

        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);

        CurriculumDataMock
            .DidNotReceive()
            .DeleteSkill(Arg.Any<string>());
    }

    [Fact]
    public void GetAll_DataContainsSkills_ShouldReturnAllSkills()
    {
        // Arrange
        CurriculumDataMock.Skills
            .Returns(FakeCurriculumData.Skills);

        var expectation = FakeCurriculumData.Skills;

        // Act
        var result = _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectation, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public void GetAll_DataDoesNotContainSkills_ShouldReturnEmptyList()
    {
        // Arrange
        CurriculumDataMock.Skills
            .Returns([]);

        // Act
        var result = _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void GetById_DataContainsSkillWithMatchingId_ShouldReturnSkillWithMatchingId()
    {
        // Arrange
        var id = FakeCurriculumData.Skills[0].Id;

        CurriculumDataMock.Skills
            .Returns(FakeCurriculumData.Skills);

        var expectation = FakeCurriculumData.Skills[0];

        // Act
        var result = _uut.GetById(id);

        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public void GetById_DataDoesNotContainSkillWithMatchingId_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Skills[0].Id;

        CurriculumDataMock.Skills
            .Returns([]);

        var expectation = new SkillNotFoundError(id);

        // Act
        var result = _uut.GetById(id);

        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}