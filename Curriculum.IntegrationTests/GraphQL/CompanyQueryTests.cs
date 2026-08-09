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
    public async Task Company_DataContainsCompanyWithMatchingId_ShouldReturnCompany()
    {
        // Arrange
        CurriculumDataMock.Companies
            .Returns(FakeCurriculumData.Companies);
        
        var expectation = FakeCurriculumData.Companies[0];
        
        var request = new GraphQLRequest
        {
            Query = """
                    query ($id: ID!) {
                      company(id: $id) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { id = expectation.Id }
        };
        
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
    public async Task Company_DataDoesNotContainCompanyWithMatchingId_ShouldReturnNotFoundError()
    {
        // Arrange
        CurriculumDataMock.Companies
            .Returns(FakeCurriculumData.Companies);
        
        var missingId = Guid.CreateVersion7();
        var request = new GraphQLRequest
        {
            Query = """
                    query ($id: ID!) {
                      company(id: $id) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { id = missingId }
        };

        var expectation = new CompanyNotFoundError(missingId);
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<CompanyResponse>(request);
        
        // Assert
        response.Errors
            .Should()
            .NotBeNullOrEmpty();
        
        response.Errors![0].Message
            .Should()
            .BeEquivalentTo(expectation.Message);
        
        response.Data.Company
            .Should()
            .BeNull();
    }

    private sealed record CompaniesResponses(Company[] Companies);

    private sealed record CompanyResponse(Company? Company);
}

