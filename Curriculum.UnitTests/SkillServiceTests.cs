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
    public void Delete_ByName_SkillExists_ShouldReturnDeletedSkill()
    {
        // Arrange
        const string name = "C#";
        
        var deletedSkill = new Skill
        {
            Id = Guid.CreateVersion7(),
            Name = name
        };
       
        CurriculumDataMock
            .DeleteSkill(null, name)
            .Returns(deletedSkill);
       
        // Act
        var result = _uut.Delete(null, name);
       
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(deletedSkill);
       
        CurriculumDataMock
            .Received(1)
            .DeleteSkill(null, name);
    }
    [Fact]
    public void Delete_ById_SkillExists_ShouldReturnDeletedSkill()
    {
        // Arrange
        var deletedSkill = new Skill
        {
            Id = Guid.CreateVersion7(),
            Name = "C#"
        };
        
        CurriculumDataMock
            .DeleteSkill(deletedSkill.Id, null)
            .Returns(deletedSkill);
       
        // Act
        var result = _uut.Delete(deletedSkill.Id, null);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(deletedSkill);
        
        CurriculumDataMock
            .Received(1)
            .DeleteSkill(deletedSkill.Id, null);
    }
    [Fact]
    public void Delete_ByName_SkillDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        const string name = "Missing";
       
        CurriculumDataMock
            .DeleteSkill(null, name)
            .Returns((Skill?)null);
      
        var expectation = new SkillNotFoundError(name);
       
        // Act
        var result = _uut.Delete(null, name);
       
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
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
    public void Get_ById_DataContainsSkill_ShouldReturnSkill()
    {
        // Arrange
        var id = FakeCurriculumData.Skills[0].Id;
        
        CurriculumDataMock.Skills.Returns(FakeCurriculumData.Skills);
        
        var expectation = FakeCurriculumData.Skills[0];
        
        // Act
        var result = _uut.Get(id, null);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void Get_ById_DataDoesNotContainSkill_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Skills[0].Id;
        
        CurriculumDataMock.Skills.Returns([]);
        
        var expectation = new SkillNotFoundError(id);
        
        // Act
        var result = _uut.Get(id, null);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void Get_ByName_DataContainsSkill_ShouldReturnSkill()
    {
        // Arrange
        var name = FakeCurriculumData.Skills[0].Name;
        
        CurriculumDataMock.Skills.Returns(FakeCurriculumData.Skills);
        
        var expectation = FakeCurriculumData.Skills[0];
        
        // Act
        var result = _uut.Get(null, name);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void Get_ByName_DataDoesNotContainSkill_ShouldReturnNotFoundError()
    {
        // Arrange
        const string name = "Missing";
        
        CurriculumDataMock.Skills.Returns(FakeCurriculumData.Skills);
        
        var expectation = new SkillNotFoundError(name);
        
        // Act
        var result = _uut.Get(null, name);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}