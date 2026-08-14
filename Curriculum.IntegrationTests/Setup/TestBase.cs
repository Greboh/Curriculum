using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Curriculum.IntegrationTests.Setup;

[Collection(IntegrationCollection.Name)]
public class TestBase : IAsyncLifetime
{
    protected readonly ApiWebApplicationFactory Factory;
    protected readonly TestServer Server;
    protected readonly IServiceScope Scope;
    protected readonly HttpClient HttpClient;
    protected readonly GraphQLHttpClient GraphQLClient;
    
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
    }
    
    public async Task InitializeAsync()
        => await Factory.ResetDatabase();

    public Task DisposeAsync()
    {
        Scope.Dispose();
        HttpClient.Dispose();
        GraphQLClient.Dispose();
        
        return Task.CompletedTask;
    }
}