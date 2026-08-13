using Curriculum.Core.Entities;
using Curriculum.IntegrationTests.Setup;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using FluentAssertions;
using GraphQL;
using Xunit;

namespace Curriculum.IntegrationTests.GraphQL;

public class ProjectQueryTests(ApiWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Projects_DataContainsProjects_ShouldReturnAllProjects()
    {
        // Arrange
        await Factory.Seed(async db =>
        {
            await db.Projects.AddRangeAsync(FakeCurriculumData.Projects);
        });

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
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Projects_DataDoesNotContainProjects_ShouldReturnEmptyList()
    {
        // Arrange
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
    public async Task Project_ById_ShouldReturnProject()
    {
        // Arrange
        await Factory.Seed(async db =>
        {
            await db.Projects.AddRangeAsync(FakeCurriculumData.Projects);
        });
        
        var expectation = FakeCurriculumData.Projects[0];
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: ByIdOrName!) {
                      project(by: $by) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { by = new { id = expectation.Id } }
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
    public async Task Project_ByName_ShouldReturnProject()
    {
        // Arrange
        await Factory.Seed(async db =>
        {
            await db.Projects.AddRangeAsync(FakeCurriculumData.Projects);
        });
        
        var expectation = FakeCurriculumData.Projects[0];
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: ByIdOrName!) {
                      project(by: $by) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { by = new { name = expectation.Name } }
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
    public async Task Project_ById_Missing_ShouldReturnNotFoundError()
    {
        // Arrange
        await Factory.Seed(async db =>
        {
            await db.Projects.AddRangeAsync(FakeCurriculumData.Projects);
        });
        
        var missingId = Guid.CreateVersion7();
        var expectation = new ProjectNotFoundError(missingId);
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: ByIdOrName!) {
                      project(by: $by) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { by = new { id = missingId } }
        };
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<ProjectResponse>(request);

        // Assert
        response.Errors
            .Should().
            NotBeNullOrEmpty();
        
        response.Errors![0].Message
            .Should()
            .Be(expectation.Message);
        
        response.Data.Project.
            Should()
            .BeNull();
    }

    private sealed record ProjectsResponses(Project[] Projects);

    private sealed record ProjectResponse(Project? Project);
}