using Curriculum.Core.Entities;
using Curriculum.IntegrationTests.Setup;
using Curriculum.Services.Errors;
using FluentAssertions;
using GraphQL;
using NSubstitute;
using Xunit;

namespace Curriculum.IntegrationTests.GraphQL.Mutations;

public class SkillMutationTests(ApiWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task CreateSkill_NameIsValid_ShouldReturnCreatedSkill()
    {
        // Arrange
        const string name = "GraphQL";
        CurriculumDataMock
            .CreateSkill(Arg.Any<Skill>())
            .Returns(x => x.Arg<Skill>());

        var request = new GraphQLRequest
        {
            Query =
                """
                mutation ($name: String!) {
                  createSkill(name: $name) {
                    id
                    name
                  }
                }
                """,
            Variables = new { name }
        };

        var expectation = new CreateSkillResponse(new()
        {
            Id = Guid.Empty,
            Name = name
        });

        // Act
        var response = await GraphQLClient.SendQueryAsync<CreateSkillResponse>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();

        response.Data
            .Should()
            .BeEquivalentTo(expectation, opt => opt.Excluding(x => x.CreateSkill.Id));

        response.Data.CreateSkill.Id
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateSkill_NameIsNullOrEmpty_ShouldReturnValidationError()
    {
        // Arrange
        const string name = "";
        var request = new GraphQLRequest
        {
            Query =
                """
                mutation ($name: String!) {
                  createSkill(name: $name) {
                    id
                    name
                  }
                }
                """,
            Variables = new { name }
        };

        var expectation = new SkillValidationError(
            name,
            new Dictionary<string, object>
            {
                { "Name", "Is Null or Empty" }
            }
        );

        // Act
        var response = await GraphQLClient.SendQueryAsync<CreateSkillResponse>(request);

        // Assert
        response.Errors
            .Should()
            .NotBeNullOrEmpty();

        response.Errors![0].Message
            .Should()
            .Be(expectation.Message);

        response.Data.CreateSkill
            .Should()
            .BeNull();
    }

    [Fact]
    public async Task DeleteSkill_SkillExists_ShouldReturnDeletedSkill()
    {
        // Arrange
        const string name = "C#";
        var deletedSkill = new Skill
        {
            Id = Guid.CreateVersion7(),
            Name = name
        };
        
        var request = new GraphQLRequest
        {
            Query =
                """
                mutation ($name: String!) {
                  deleteSkill(name: $name) {
                    id
                    name
                  }
                }
                """,
            Variables = new { name }
        };
        CurriculumDataMock
            .DeleteSkill(name)
            .Returns(deletedSkill);
        var expectation = new DeleteSkillResponse(deletedSkill);
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<DeleteSkillResponse>(request);
        
        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();
        
        response.Data
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task DeleteSkill_SkillDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        const string name = "Missing";
        var request = new GraphQLRequest
        {
            Query =
                """
                mutation ($name: String!) {
                  deleteSkill(name: $name) {
                    id
                    name
                  }
                }
                """,
            Variables = new { name }
        };
        
        CurriculumDataMock
            .DeleteSkill(name)
            .Returns((Skill?)null);
        
        var expectation = new SkillNotFoundError(name);
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<DeleteSkillResponse>(request);
        
        // Assert
        response.Errors
            .Should()
            .NotBeNullOrEmpty();
        
        response.Errors![0].Message
            .Should()
            .Be(expectation.Message);
        
        response.Data.DeleteSkill
            .Should()
            .BeNull();
    }

    [Fact]
    public async Task DeleteSkill_NameIsNullOrEmpty_ShouldReturnValidationError()
    {
        // Arrange
        const string name = "";
        var request = new GraphQLRequest
        {
            Query =
                """
                mutation ($name: String!) {
                  deleteSkill(name: $name) {
                    id
                    name
                  }
                }
                """,
            Variables = new { name }
        };
        
        var expectation = new SkillValidationError(
            name,
            new Dictionary<string, object>
            {
                { "Name", "Is Null or Empty" }
                
            });
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<DeleteSkillResponse>(request);
        
        // Assert
        response.Errors
            .Should()
            .NotBeNullOrEmpty();
        
        response.Errors![0].Message
            .Should()
            .Be(expectation.Message);
        
        response.Data.DeleteSkill
            .Should()
            .BeNull();
    }

    private sealed record CreateSkillResponse(Skill CreateSkill);

    private sealed record DeleteSkillResponse(Skill? DeleteSkill);
}