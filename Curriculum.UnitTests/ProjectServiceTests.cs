using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
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
            .BeEmpty();
    }
    
    [Fact]
    public void Get_ById_DataContainsProject_ShouldReturnProject()
    {
        // Arrange
        var id = FakeCurriculumData.Projects[0].Id;
        
        CurriculumDataMock.Projects.Returns(FakeCurriculumData.Projects);
        
        var expectation = FakeCurriculumData.Projects[0];
        
        // Act
        var result = _uut.Get(id, null);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void Get_ById_DataDoesNotContainProject_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Projects[0].Id;
        
        CurriculumDataMock.Projects.Returns([]);
        
        var expectation = new ProjectNotFoundError(id);
        
        // Act
        var result = _uut.Get(id, null);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void Get_ByName_DataContainsProject_ShouldReturnProject()
    {
        // Arrange
        var name = FakeCurriculumData.Projects[0].Name;
        
        CurriculumDataMock.Projects.Returns(FakeCurriculumData.Projects);
        
        var expectation = FakeCurriculumData.Projects[0];
        
        // Act
        var result = _uut.Get(null, name);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void Get_ByName_DataDoesNotContainProject_ShouldReturnNotFoundError()
    {
        // Arrange
        const string name = "Missing";
        
        CurriculumDataMock.Projects.Returns(FakeCurriculumData.Projects);
        
        var expectation = new ProjectNotFoundError(name);
        
        // Act
        var result = _uut.Get(null, name);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}