using Curriculum.Core.Entities;
using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.UnitTests.Fakes;
using Curriculum.UnitTests.Setup;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Curriculum.UnitTests;

public class ProjectServiceTests : TestBase
{
    private readonly ProjectService _uut;

    public ProjectServiceTests()
    {
        _uut = new(CurriculumDataMock);
    }

    [Fact]
    public void GetAll_DataContainsProjects_ShouldReturnAllProjects()
    {
        // Arrange
        CurriculumDataMock.Projects
            .Returns(FakeCurriculumData.Projects);

        var expectation = FakeCurriculumData.Projects;
        
        // Act
        var result = _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectation, opt => opt.WithStrictOrdering());
    }
    
    [Fact]
    public void GetById_DataContainsProjectWithMatchingId_ShouldReturnProjectWithMatchingId()
    {
        // Arrange
        var id = FakeCurriculumData.Projects[0].Id;
        
        CurriculumDataMock.Projects
            .Returns(FakeCurriculumData.Projects);

        var expectation = FakeCurriculumData.Projects[0];
        
        // Act
        var result = _uut.GetById(id);

        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void GetAll_DataDoesNotContainProjects_ShouldReturnEmptyList()
    {
        // Arrange
        CurriculumDataMock.Projects
            .Returns([]);
        
        // Act
        var result = _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEquivalentTo(new List<Project>(), opt => opt.WithStrictOrdering());
    }
    
    [Fact]
    public void GetById_DataDoesNotContainProjectWithMatchingId_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Projects[0].Id;
        
        CurriculumDataMock.Projects
            .Returns([]);

        var expectation = new ProjectNotFoundError(id);
        
        // Act
        var result = _uut.GetById(id);

        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}