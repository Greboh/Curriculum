using Curriculum.Core.Entities;
using Curriculum.IntegrationTests.Setup;
using Curriculum.Services.Errors;
using FluentAssertions;
using GraphQL;
using Xunit;

namespace Curriculum.IntegrationTests.GraphQL.Mutations;

public class SkillMutationTests(ApiWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task CreateSkill_NameIsValid_ShouldReturnCreatedSkill()
    {
        // Arrange
        const string name = "GraphQL";

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
    public async Task DeleteSkill_ByName_SkillExists_ShouldReturnDeletedSkill()
    {
        // Arrange
        const string name = "C#";
        await Factory.Seed(async db =>
        {
            await db.Skills.AddAsync(new()
            {
                Id = Guid.Empty,
                Name = name
            });
        });
        
        var request = new GraphQLRequest
        {
            Query = """
                    mutation ($by: ByIdOrName!) {
                      deleteSkill(by: $by)
                    }
                    """,
            Variables = new { by = new { name } }
        };
        
        var expectation = new DeleteSkillResponse(true);

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
    public async Task DeleteSkill_ByName_SkillDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        const string name = "Missing";
        var request = new GraphQLRequest
        {
            Query = """
                    mutation ($by: ByIdOrName!) {
                      deleteSkill(by: $by)
                    }
                    """,
            Variables = new { by = new { name } }
        };
        
        var expectation = new DeleteSkillResponse(false);

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
    public async Task DeleteSkill_ById_SkillExists_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.CreateVersion7();
        await Factory.Seed(async db =>
        {
            await db.Skills.AddAsync(new()
            {
                Id = id,
                Name = "Test"
            });
        });
        
        var request = new GraphQLRequest
        {
            Query = """
                    mutation ($by: ByIdOrName!) {
                      deleteSkill(by: $by)
                    }
                    """,
            Variables = new { by = new { id } }
        };
        
        var expectation = new DeleteSkillResponse(true);

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
    public async Task DeleteSkill_ById_SkillDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.Empty;
        var request = new GraphQLRequest
        {
            Query = """
                    mutation ($by: ByIdOrName!) {
                      deleteSkill(by: $by)
                    }
                    """,
            Variables = new { by = new { id } }
        };

        var expectation = new DeleteSkillResponse(false);

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

    private sealed record CreateSkillResponse(Skill CreateSkill);

    private sealed record DeleteSkillResponse(bool DeleteSkill);
}