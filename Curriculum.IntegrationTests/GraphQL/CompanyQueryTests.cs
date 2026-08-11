using Curriculum.Core.Entities;
using Curriculum.IntegrationTests.Setup;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using FluentAssertions;
using GraphQL;
using NSubstitute;
using Xunit;

namespace Curriculum.IntegrationTests.GraphQL;

public class CompanyQueryTests(ApiWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Companies_DataContainsCompanies_ShouldReturnAllCompanies()
    {
        // Arrange
        CurriculumDataMock.Companies
            .Returns(FakeCurriculumData.Companies);

        var request = new GraphQLRequest
        {
            Query =
                """
                query {
                  companies {
                    id
                    name
                  }
                }
                """
        };

        var expectation = FakeCurriculumData.Companies;

        // Act
        var response = await GraphQLClient.SendQueryAsync<CompaniesResponses>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();

        response.Data.Companies
            .Should()
            .BeEquivalentTo(expectation, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Companies_DataDoesNotContainCompanies_ShouldReturnEmptyList()
    {
        // Arrange
        CurriculumDataMock.Companies
            .Returns([]);

        var request = new GraphQLRequest
        {
            Query =
                """
                query {
                  companies {
                    id
                    name
                  }
                }
                """
        };

        // Act
        var response = await GraphQLClient.SendQueryAsync<CompaniesResponses>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();

        response.Data.Companies
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Company_ById_ShouldReturnCompany()
    {
        // Arrange
        var expectation = FakeCurriculumData.Companies[0];
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: ByIdOrName!) {
                      company(by: $by) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { by = new { id = expectation.Id } }
        };

        CurriculumDataMock.Companies.Returns(FakeCurriculumData.Companies);
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<CompanyResponse>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();
        
        response.Data.Company
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Company_ByName_ShouldReturnCompany()
    {
        // Arrange
        var expectation = FakeCurriculumData.Companies[0];
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: ByIdOrName!) {
                      company(by: $by) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { by = new { name = expectation.Name } }
        };

        CurriculumDataMock.Companies.Returns(FakeCurriculumData.Companies);
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<CompanyResponse>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();
        
        response.Data.Company
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Company_ById_Missing_ShouldReturnNotFoundError()
    {
        // Arrange
        var missingId = Guid.CreateVersion7();
        var expectation = new CompanyNotFoundError(missingId);
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: ByIdOrName!) {
                      company(by: $by) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { by = new { id = missingId } }
        };

        CurriculumDataMock.Companies.Returns(FakeCurriculumData.Companies);
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<CompanyResponse>(request);

        // Assert
        response.Errors
            .Should().
            NotBeNullOrEmpty();
        
        response.Errors![0].Message
            .Should()
            .Be(expectation.Message);
        
        response.Data.Company.
            Should()
            .BeNull();
    }

    private sealed record CompaniesResponses(Company[] Companies);

    private sealed record CompanyResponse(Company? Company);
}