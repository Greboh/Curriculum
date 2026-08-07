using Curriculum.Core.Entities;
using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.UnitTests.Fakes;
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
            .BeEquivalentTo(new List<Skill>(), opt => opt.WithStrictOrdering());
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