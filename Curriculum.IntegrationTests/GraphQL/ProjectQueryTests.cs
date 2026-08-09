using Curriculum.Core.Entities;
using Curriculum.IntegrationTests.Setup;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using FluentAssertions;
using GraphQL;
using NSubstitute;
using Xunit;

namespace Curriculum.IntegrationTests.GraphQL;

public class ProjectQueryTests(ApiWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Projects_DataContainsProjects_ShouldReturnAllProjects()
    {
        // Arrange
        CurriculumDataMock.Projects
            .Returns(FakeCurriculumData.Projects);

        var request = new GraphQLRequest
        {
            Query =
                """
                query {
                  projects {
                    id
                    name
                  }
                }
                """
        };

        var expectation = FakeCurriculumData.Projects;

        // Act
        var response = await GraphQLClient.SendQueryAsync<ProjectsResponses>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();

        response.Data.Projects
            .Should()
            .BeEquivalentTo(expectation, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Projects_DataDoesNotContainProjects_ShouldReturnEmptyList()
    {
        // Arrange
        CurriculumDataMock.Projects
            .Returns([]);

        var request = new GraphQLRequest
        {
            Query =
                """
                query {
                  projects {
                    id
                    name
                  }
                }
                """
        };

        // Act
        var response = await GraphQLClient.SendQueryAsync<ProjectsResponses>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();

        response.Data.Projects
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Project_DataContainsProjectWithMatchingId_ShouldReturnProject()
    {
        // Arrange
        CurriculumDataMock.Projects
            .Returns(FakeCurriculumData.Projects);

        var expectation = FakeCurriculumData.Projects[0];

        var request = new GraphQLRequest
        {
            Query = """
                    query ($id: ID!) {
                      project(id: $id) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { id = expectation.Id }
        };

        // Act
        var response = await GraphQLClient.SendQueryAsync<ProjectResponse>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();

        response.Data.Project
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Project_DataDoesNotContainProjectWithMatchingId_ShouldReturnNotFoundError()
    {
        // Arrange
        CurriculumDataMock.Projects
            .Returns(FakeCurriculumData.Projects);

        var missingId = Guid.CreateVersion7();
        var request = new GraphQLRequest
        {
            Query = """
                    query ($id: ID!) {
                      project(id: $id) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { id = missingId }
        };

        var expectation = new ProjectNotFoundError(missingId);

        // Act
        var response = await GraphQLClient.SendQueryAsync<ProjectResponse>(request);

        // Assert
        response.Errors
            .Should()
            .NotBeNullOrEmpty();

        response.Errors![0].Message
            .Should()
            .BeEquivalentTo(expectation.Message);

        response.Data.Project
            .Should()
            .BeNull();
    }

    private sealed record ProjectsResponses(Project[] Projects);

    private sealed record ProjectResponse(Project? Project);
}