using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using Curriculum.UnitTests.Setup;
using FluentAssertions;
using Xunit;

namespace Curriculum.UnitTests;

public class ProjectServiceTests : TestBase
{
    private readonly ProjectService _uut;

    public ProjectServiceTests()
    {
        _uut = new(Context);
    }

    [Fact]
    public async Task GetAll_DataContainsProjects_ShouldReturnAllProjects()
    {
        // Arrange
        Context.Projects.AddRange(FakeCurriculumData.Projects);
        await Context.SaveChangesAsync();
        
        var expectation = FakeCurriculumData.Projects;
        
        // Act
        var result = await _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task GetAll_DataDoesNotContainProjects_ShouldReturnEmptyList()
    {
        // Act
        var result = await _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEmpty();
    }
    
    [Fact]
    public async Task Get_ById_DataContainsProject_ShouldReturnProject()
    {
        // Arrange
        var id = FakeCurriculumData.Projects[0].Id;
        
        Context.Projects.AddRange(FakeCurriculumData.Projects);
        await Context.SaveChangesAsync();
        
        var expectation = FakeCurriculumData.Projects[0];
        
        // Act
        var result = await _uut.Get(id, null);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public async Task Get_ById_DataDoesNotContainProject_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Projects[0].Id;
        
        var expectation = new ProjectNotFoundError(id);
        
        // Act
        var result = await _uut.Get(id, null);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public async Task Get_ByName_DataContainsProject_ShouldReturnProject()
    {
        // Arrange
        var name = FakeCurriculumData.Projects[0].Name;
        
        Context.Projects.AddRange(FakeCurriculumData.Projects);
        await Context.SaveChangesAsync();
        
        var expectation = FakeCurriculumData.Projects[0];
        
        // Act
        var result = await _uut.Get(null, name);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public async Task Get_ByName_DataDoesNotContainProject_ShouldReturnNotFoundError()
    {
        // Arrange
        const string name = "Missing";
        
        Context.Projects.AddRange(FakeCurriculumData.Projects);
        await Context.SaveChangesAsync();
        
        var expectation = new ProjectNotFoundError(name);
        
        // Act
        var result = await _uut.Get(null, name);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}