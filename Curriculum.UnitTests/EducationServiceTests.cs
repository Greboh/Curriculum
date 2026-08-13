using Curriculum.Core.Entities;
using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using Curriculum.UnitTests.Setup;
using FluentAssertions;
using Xunit;

namespace Curriculum.UnitTests;

public class EducationServiceTests : TestBase
{
    private readonly EducationService _uut;

    public EducationServiceTests()
    {
        _uut = new(Context);
    }

    [Fact]
    public async Task GetAll_DataContainsEducations_ShouldReturnAllEducations()
    {
        // Arrange
        Context.Educations.AddRange(FakeCurriculumData.Educations);
        await Context.SaveChangesAsync();
        
        var expectation = FakeCurriculumData.Educations;
        
        // Act
        var result = await _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task GetAll_DataDoesNotContainEducations_ShouldReturnEmptyList()
    {
        // Act
        var result = await _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Get_ById_DataContainsEducation_ShouldReturnEducation()
    {
        // Arrange
        var id = FakeCurriculumData.Educations[0].Id;
        
        Context.Educations.AddRange(FakeCurriculumData.Educations);
        await Context.SaveChangesAsync();
        
        var expectation = FakeCurriculumData.Educations[0];
        
        // Act
        var result = await _uut.Get(id, null);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    [Fact]
    public async Task Get_ById_DataDoesNotContainEducation_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Educations[0].Id;
        
        var expectation = new EducationNotFoundError(id);
        
        // Act
        var result = await _uut.Get(id, null);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
    [Fact]
    public async Task Get_ByInstitution_DataContainsEducation_ShouldReturnEducation()
    {
        // Arrange
        var institution = FakeCurriculumData.Educations[0].Institution;
        
        Context.Educations.AddRange(FakeCurriculumData.Educations);
        await Context.SaveChangesAsync();
        
        var expectation = FakeCurriculumData.Educations[0];
        
        // Act
        var result = await _uut.Get(null, institution);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public async Task Get_ByInstitution_DataDoesNotContainEducation_ShouldReturnNotFoundError()
    {
        // Arrange
        const string institution = "Missing";
        
        Context.Educations.AddRange(FakeCurriculumData.Educations);
        await Context.SaveChangesAsync();
        
        var expectation = new EducationNotFoundError(institution);
        
        // Act
        var result = await _uut.Get(null, institution);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}