using Curriculum.Core.Entities;
using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.UnitTests.Fakes;
using Curriculum.UnitTests.Setup;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Curriculum.UnitTests;

public class CompanyServiceTests : TestBase
{
    private readonly CompanyService _uut;

    public CompanyServiceTests()
    {
        _uut = new(CurriculumDataMock);
    }

    [Fact]
    public void GetAll_DataContainsCompanies_ShouldReturnAllCompanies()
    {
        // Arrange
        CurriculumDataMock.Companies
            .Returns(FakeCurriculumData.Companies);

        var expectation = FakeCurriculumData.Companies;
        
        // Act
        var result = _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectation, opt => opt.WithStrictOrdering());
    }
    
    [Fact]
    public void GetById_DataContainsCompanyWithMatchingId_ShouldReturnCompanyWithMatchingId()
    {
        // Arrange
        var id = FakeCurriculumData.Companies[0].Id;
        
        CurriculumDataMock.Companies
            .Returns(FakeCurriculumData.Companies);

        var expectation = FakeCurriculumData.Companies[0];
        
        // Act
        var result = _uut.GetById(id);

        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void GetAll_DataDoesNotContainCompanies_ShouldReturnEmptyList()
    {
        // Arrange
        CurriculumDataMock.Companies
            .Returns([]);
        
        // Act
        var result = _uut.GetAll();

        // Assert
        result
            .Should()
            .BeEquivalentTo(new List<Company>(), opt => opt.WithStrictOrdering());
    }
    
    [Fact]
    public void GetById_DataDoesNotContainCompanyWithMatchingId_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Companies[0].Id;
        
        CurriculumDataMock.Companies
            .Returns([]);

        var expectation = new CompanyNotFoundError(id);
        
        // Act
        var result = _uut.GetById(id);

        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}