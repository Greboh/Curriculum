using Curriculum.Core.Entities;
using Curriculum.IntegrationTests.Setup;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using FluentAssertions;
using GraphQL;
using Xunit;

namespace Curriculum.IntegrationTests.GraphQL;

public class EducationQueryTests(ApiWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Educations_DataContainsEducations_ShouldReturnAllEducations()
    {
        // Arrange
        await Factory.Seed(async db =>
        {
            await db.Educations.AddRangeAsync(FakeCurriculumData.Educations);
        });

        var request = new GraphQLRequest
        {
            Query =
                """
                query {
                  educations {
                    id
                    institution
                    degree
                    startDate
                    endDate
                  }
                }
                """
        };

        var expectation = FakeCurriculumData.Educations;

        // Act
        var response = await GraphQLClient.SendQueryAsync<EducationsResponses>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();

        response.Data.Educations
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Educations_DataDoesNotContainEducations_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GraphQLRequest
        {
            Query =
                """
                query {
                  educations {
                    id
                    institution
                    degree
                    startDate
                    endDate
                  }
                }
                """
        };

        // Act
        var response = await GraphQLClient.SendQueryAsync<EducationsResponses>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();

        response.Data.Educations
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Education_ById_ShouldReturnEducation()
    {
        // Arrange
        await Factory.Seed(async db =>
        {
            await db.Educations.AddRangeAsync(FakeCurriculumData.Educations);
        });
        
        var expectation = FakeCurriculumData.Educations[0];
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: EducationBy!) {
                      education(by: $by) {
                        id
                        institution
                        degree
                        startDate
                        endDate
                      }
                    }
                    """,
            Variables = new { by = new { id = expectation.Id } }
        };
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<EducationResponse>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();
        
        response.Data.Education
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Education_ByName_ShouldReturnEducation()
    {
        // Arrange
        await Factory.Seed(async db =>
        {
            await db.Educations.AddRangeAsync(FakeCurriculumData.Educations);
        });
        
        var expectation = FakeCurriculumData.Educations[0];
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: EducationBy!) {
                      education(by: $by) {
                        id
                        institution
                        degree
                        startDate
                        endDate
                      }
                    }
                    """,
            Variables = new { by = new { institution = expectation.Institution } }
        };
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<EducationResponse>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();
        
        response.Data.Education
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Education_ById_Missing_ShouldReturnNotFoundError()
    {
        // Arrange
        await Factory.Seed(async db =>
        {
            await db.Educations.AddRangeAsync(FakeCurriculumData.Educations);
        });
        
        var missingId = Guid.CreateVersion7();
        var expectation = new EducationNotFoundError(missingId);
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: EducationBy!) {
                      education(by: $by) {
                        id
                        institution
                      }
                    }
                    """,
            Variables = new { by = new { id = missingId } }
        };
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<EducationResponse>(request);

        // Assert
        response.Errors
            .Should().
            NotBeNullOrEmpty();
        
        response.Errors![0].Message
            .Should()
            .Be(expectation.Message);
        
        response.Data.Education.
            Should()
            .BeNull();
    }

    private sealed record EducationsResponses(Education[] Educations);

    private sealed record EducationResponse(Education? Education);
}