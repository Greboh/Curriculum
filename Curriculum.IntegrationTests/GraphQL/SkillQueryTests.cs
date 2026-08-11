using Curriculum.Core.Entities;
using Curriculum.IntegrationTests.Setup;
using Curriculum.Services.Errors;
using Curriculum.Tests.Shared;
using FluentAssertions;
using GraphQL;
using NSubstitute;
using Xunit;

namespace Curriculum.IntegrationTests.GraphQL;

public class SkillQueryTests(ApiWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Skills_DataContainsSkills_ShouldReturnAllSkills()
    {
        // Arrange
        CurriculumDataMock.Skills
            .Returns(FakeCurriculumData.Skills);

        var request = new GraphQLRequest
        {
            Query =
                """
                query {
                  skills {
                    id
                    name
                  }
                }
                """
        };

        var expectation = FakeCurriculumData.Skills;

        // Act
        var response = await GraphQLClient.SendQueryAsync<SkillsResponses>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();

        response.Data.Skills
            .Should()
            .BeEquivalentTo(expectation, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Skills_DataDoesNotContainSkills_ShouldReturnEmptyList()
    {
        // Arrange
        CurriculumDataMock.Skills
            .Returns([]);

        var request = new GraphQLRequest
        {
            Query =
                """
                query {
                  skills {
                    id
                    name
                  }
                }
                """
        };

        // Act
        var response = await GraphQLClient.SendQueryAsync<SkillsResponses>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();

        response.Data.Skills
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Skill_ById_ShouldReturnSkill()
    {
        // Arrange
        var expectation = FakeCurriculumData.Skills[0];
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: ByIdOrName!) {
                      skill(by: $by) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { by = new { id = expectation.Id } }
        };

        CurriculumDataMock.Skills.Returns(FakeCurriculumData.Skills);
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<SkillResponse>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();
        
        response.Data.Skill
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Skill_ByName_ShouldReturnSkill()
    {
        // Arrange
        var expectation = FakeCurriculumData.Skills[0];
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: ByIdOrName!) {
                      skill(by: $by) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { by = new { name = expectation.Name } }
        };

        CurriculumDataMock.Skills.Returns(FakeCurriculumData.Skills);
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<SkillResponse>(request);

        // Assert
        response.Errors
            .Should()
            .BeNullOrEmpty();
        
        response.Data.Skill
            .Should()
            .BeEquivalentTo(expectation);
    }

    [Fact]
    public async Task Skill_ById_Missing_ShouldReturnNotFoundError()
    {
        // Arrange
        var missingId = Guid.CreateVersion7();
        var expectation = new SkillNotFoundError(missingId);
        var request = new GraphQLRequest
        {
            Query = """
                    query ($by: ByIdOrName!) {
                      skill(by: $by) {
                        id
                        name
                      }
                    }
                    """,
            Variables = new { by = new { id = missingId } }
        };

        CurriculumDataMock.Skills.Returns(FakeCurriculumData.Skills);
        
        // Act
        var response = await GraphQLClient.SendQueryAsync<SkillResponse>(request);

        // Assert
        response.Errors
            .Should().
            NotBeNullOrEmpty();
        
        response.Errors![0].Message
            .Should()
            .Be(expectation.Message);
        
        response.Data.Skill.
            Should()
            .BeNull();
    }
    
    private sealed record SkillsResponses(Skill[] Skills);

    private sealed record SkillResponse(Skill? Skill);
}