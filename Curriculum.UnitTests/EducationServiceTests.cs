using Curriculum.Core.Entities;
using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.UnitTests.Fakes;
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
    public void GetById_DataContainsEducationWithMatchingId_ShouldReturnEducationWithMatchingId()
    {
        // Arrange
        var id = FakeCurriculumData.Educations[0].Id;
        
        CurriculumDataMock.Educations
            .Returns(FakeCurriculumData.Educations);

        var expectation = FakeCurriculumData.Educations[0];
        
        // Act
        var result = _uut.GetById(id);

        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
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
            .BeEquivalentTo(new List<Education>(), opt => opt.WithStrictOrdering());
    }
    
    [Fact]
    public void GetById_DataDoesNotContainEducationWithMatchingId_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Educations[0].Id;
        
        CurriculumDataMock.Educations
            .Returns([]);

        var expectation = new EducationNotFoundError(id);
        
        // Act
        var result = _uut.GetById(id);

        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}