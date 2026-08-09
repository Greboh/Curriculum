using Microsoft.Extensions.DependencyInjection;

namespace Curriculum.IntegrationTests.Extensions;

public static class IServiceCollectionExtensions
{
    public static void MockService<TService>(this IServiceCollection serviceCollection)
        where TService : class
    {
        var descriptor = serviceCollection.FirstOrDefault(d => d.ServiceType == typeof(TService));

        if (descriptor == null)
        {
            return;
        }

        serviceCollection.Remove(descriptor);

        var mockInstance = NSubstitute.Substitute.For<TService>();
        serviceCollection.AddSingleton(mockInstance);
    }
}