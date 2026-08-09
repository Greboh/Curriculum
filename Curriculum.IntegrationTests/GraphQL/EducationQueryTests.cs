using Curriculum.Core.Entities;
using Curriculum.IntegrationTests.Setup;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using FluentAssertions;
using GraphQL;
using NSubstitute;
using Xunit;

namespace Curriculum.IntegrationTests.GraphQL;

public class EducationQueryTests(ApiWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Educations_DataContainsEducations_ShouldReturnAllEducations()
    {
        // Arrange
        CurriculumDataMock.Educations
            .Returns(FakeCurriculumData.Educations);

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
            .BeEquivalentTo(expectation, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Educations_DataDoesNotContainEducations_ShouldReturnEmptyList()
    {
        // Arrange
        CurriculumDataMock.Educations
            .Returns([]);

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
    public async Task Education_DataContainsEducationWithMatchingId_ShouldReturnEducation()
    {
        // Arrange
        CurriculumDataMock.Educations
            .Returns(FakeCurriculumData.Educations);

        var expectation = FakeCurriculumData.Educations[0];

        var request = new GraphQLRequest
        {
            Query = """
                    query ($id: ID!) {
                      education(id: $id) {
                        id
                        institution
                        degree
                        startDate
                        endDate
                      }
                    }
                    """,
            Variables = new { id = expectation.Id }
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
    public async Task Education_DataDoesNotContainEducationWithMatchingId_ShouldReturnNotFoundError()
    {
        // Arrange
        CurriculumDataMock.Educations
            .Returns(FakeCurriculumData.Educations);

        var missingId = Guid.CreateVersion7();
        var request = new GraphQLRequest
        {
            Query = """
                    query ($id: ID!) {
                      education(id: $id) {
                        id
                        institution
                        degree
                        startDate
                        endDate
                      }
                    }
                    """,
            Variables = new { id = missingId }
        };

        var expectation = new EducationNotFoundError(missingId);

        // Act
        var response = await GraphQLClient.SendQueryAsync<EducationResponse>(request);

        // Assert
        response.Errors
            .Should()
            .NotBeNullOrEmpty();

        response.Errors![0].Message
            .Should()
            .BeEquivalentTo(expectation.Message);

        response.Data.Education
            .Should()
            .BeNull();
    }

    private sealed record EducationsResponses(Education[] Educations);

    private sealed record EducationResponse(Education? Education);
}