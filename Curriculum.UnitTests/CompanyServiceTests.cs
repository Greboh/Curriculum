using Curriculum.Core.Entities;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using Curriculum.UnitTests.Setup;
using FluentAssertions;
using Xunit;

namespace Curriculum.UnitTests;

public class CompanyServiceTests : TestBase
{
    private CompanyService _uut;

    public CompanyServiceTests()
    {
        _uut = new(Context);
    }
    
    [Fact]
    public async Task GetAll_DataContainsCompanies_ShouldReturnAllCompanies()
    {
        // Arrange
        Context.Companies.AddRange(FakeCurriculumData.Companies);
        await Context.SaveChangesAsync();

        // Act
        var result = await _uut.GetAll();

        // Assert
        result.Should().BeEquivalentTo(FakeCurriculumData.Companies);
    }

    [Fact]
    public async Task GetAll_DataDoesNotContainCompanies_ShouldReturnEmptyList()
    {
        // Act
        var result = await _uut.GetAll();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_ById_DataContainsCompany_ShouldReturnCompany()
    {
        // Arrange
        var company = FakeCurriculumData.Companies[0];
        Context.Companies.Add(company);
        await Context.SaveChangesAsync();

        // Act
        var result = await _uut.Get(company.Id, null);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(company);
    }

    [Fact]
    public async Task Get_ById_Missing_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.CreateVersion7();

        // Act
        var result = await _uut.Get(id, null);

        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyNotFoundError(id));
    }

    [Fact]
    public async Task Get_ByName_DataContainsCompany_ShouldReturnCompany()
    {
        // Arrange
        var company = FakeCurriculumData.Companies[0];
        Context.Companies.Add(company);
        await Context.SaveChangesAsync();

        // Act
        var result = await _uut.Get(null, company.Name);

        // Assert
        result.Value.Should().BeEquivalentTo(company);
    }

    [Fact]
    public async Task Get_ByName_Missing_ShouldReturnNotFoundError()
    {
        // Arrange
        const string name = "Missing";

        // Act
        var result = await _uut.Get(null, name);

        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyNotFoundError(name));
    }
}