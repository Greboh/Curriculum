using Curriculum.Infrastructure.Persistence;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute.ClearExtensions;
using Xunit;

namespace Curriculum.IntegrationTests.Setup;

public class TestBase : IClassFixture<ApiWebApplicationFactory>, IDisposable
{
    protected readonly ApiWebApplicationFactory Factory;
    protected readonly TestServer Server;
    protected readonly IServiceScope Scope;
    protected readonly HttpClient HttpClient;
    protected readonly GraphQLHttpClient GraphQLClient;
    
    protected readonly ICurriculumData CurriculumDataMock;

    public TestBase(ApiWebApplicationFactory factory)
    {
        Factory = factory;
        Server = factory.Server;
        Scope = Server.Services.CreateScope();
        HttpClient = factory.CreateClient();

        GraphQLClient = new(
            new GraphQLHttpClientOptions
            {
                EndPoint = new(HttpClient.BaseAddress!, "graphql")
            },
            new SystemTextJsonSerializer(),
            HttpClient
        );
        
        CurriculumDataMock = Scope.ServiceProvider.GetRequiredService<ICurriculumData>();
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Scope.Dispose();
        HttpClient.Dispose();
        GraphQLClient.Dispose();
        ClearMocks();
    }

    private void ClearMocks()
    {
        CurriculumDataMock.ClearSubstitute();
    }
}