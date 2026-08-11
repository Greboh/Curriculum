using Curriculum.Core.Entities;
using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
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
            .BeEmpty();
    }

    [Fact]
    public void Get_ById_DataContainsCompany_ShouldReturnCompany()
    {
        // Arrange
        var id = FakeCurriculumData.Companies[0].Id;
        
        CurriculumDataMock.Companies.Returns(FakeCurriculumData.Companies);
        
        var expectation = FakeCurriculumData.Companies[0];
        
        // Act
        var result = _uut.Get(id, null);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void Get_ById_DataDoesNotContainCompany_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = FakeCurriculumData.Companies[0].Id;
        
        CurriculumDataMock.Companies.Returns([]);
        
        var expectation = new CompanyNotFoundError(id);
        
        // Act
        var result = _uut.Get(id, null);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void Get_ByName_DataContainsCompany_ShouldReturnCompany()
    {
        // Arrange
        var name = FakeCurriculumData.Companies[0].Name;
        
        CurriculumDataMock.Companies.Returns(FakeCurriculumData.Companies);
        
        var expectation = FakeCurriculumData.Companies[0];
        
        // Act
        var result = _uut.Get(null, name);
        
        // Assert
        result.Value
            .Should()
            .BeEquivalentTo(expectation);
    }
    
    [Fact]
    public void Get_ByName_DataDoesNotContainCompany_ShouldReturnNotFoundError()
    {
        // Arrange
        const string name = "Missing";
        
        CurriculumDataMock.Companies.Returns(FakeCurriculumData.Companies);
        
        var expectation = new CompanyNotFoundError(name);
        
        // Act
        var result = _uut.Get(null, name);
        
        // Assert
        result.Error
            .Should()
            .BeEquivalentTo(expectation);
    }
}