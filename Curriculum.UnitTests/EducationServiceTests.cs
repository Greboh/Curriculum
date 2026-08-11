using Curriculum.Core.Entities;
using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using Curriculum.UnitTests.Setup;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Curriculum.UnitTests;

public class EducationServiceTests : TestBase
{
    private readonly EducationService _uut;

    public EducationServiceTests()
    {
        _uut = new(CurriculumDataMock);
    }

    [Fact]
    public void GetAll_DataContainsEducations_ShouldReturnAllEducations()
    {
        // Arrange
        CurriculumDataMock.Educations
            .Returns(FakeCurriculumData.Educations);

        var expectation = FakeCurriculumData.Educations;
        
        // Act
        var result = _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectation, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public void GetAll_DataDoesNotContainEducations_ShouldReturnEmptyList()
    {
        // Arrange
        CurriculumDataMock.Educations
            .Returns([]);
        
        // Act
        var result = _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void Get_ById_DataContainsEducation_ShouldReturnEducation()
    {
        // Arrange
        var id = FakeCurriculumData.Educations[0].Id;
        
        CurriculumDataMock.Educations.Returns(FakeCurriculumData.Educations);
        
        var expectation = FakeCurriculumData.Educations[0];
        
        // Act
        var result = _uut.Get(id, null);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    [Fact]
    public void Get_ById_DataDoesNotContainEducation_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Educations[0].Id;
        
        CurriculumDataMock.Educations.Returns([]);
        
        var expectation = new EducationNotFoundError(id);
        
        // Act
        var result = _uut.Get(id, null);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
    [Fact]
    public void Get_ByInstitution_DataContainsEducation_ShouldReturnEducation()
    {
        // Arrange
        var institution = FakeCurriculumData.Educations[0].Institution;
        
        CurriculumDataMock.Educations.Returns(FakeCurriculumData.Educations);
        
        var expectation = FakeCurriculumData.Educations[0];
        
        // Act
        var result = _uut.Get(null, institution);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void Get_ByInstitution_DataDoesNotContainEducation_ShouldReturnNotFoundError()
    {
        // Arrange
        const string institution = "Missing";
        
        CurriculumDataMock.Educations.Returns(FakeCurriculumData.Educations);
        
        var expectation = new EducationNotFoundError(institution);
        
        // Act
        var result = _uut.Get(null, institution);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}