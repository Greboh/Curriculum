using Xunit;

namespace Curriculum.IntegrationTests.Setup;

[CollectionDefinition(Name)]
public sealed class IntegrationCollection : ICollectionFixture<ApiWebApplicationFactory>
{
    public const string Name = "Integration";
}