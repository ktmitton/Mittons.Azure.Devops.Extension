using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Net.Http;
using Mittons.Azure.Devops.Extension.Service.Location;

namespace Mittons.Azure.Devops.Extension.Tests.Net.Http;

public class ResourceAreaUrlResolverTests
{
    [Theory]
    [InlineData("Resource Area 1", "https://localhost/first")]
    [InlineData("Resource Area 2", "https://localhost/second")]
    public async Task ResolveAsync_WhenCalledForAResourceArea_ExpectTheAssociatedUriToBeReturned(string resourceAreaId, string url)
    {
        // Arrange
        var expectedUri = new Uri(url);

        var mockLocationService = new Mock<ILocationService>();
        mockLocationService.Setup(x => x.GetResourceAreaLocationAsync(resourceAreaId))
            .ReturnsAsync(url);

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ILocationService>(mockLocationService.Object);
        serviceCollection.AddResourceAreaUriResolver();

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        var resolver = serviceProvider.GetRequiredService<IResourceAreaUriResolver>();

        // Act
        var actualUri = await resolver.ResolveAsync(resourceAreaId);

        // Assert
        Assert.Equal(expectedUri, actualUri);
    }

    [Theory]
    [InlineData("Resource Area 1", "https://localhost/first")]
    [InlineData("Resource Area 2", "https://localhost/second")]
    public void Resolve_WhenCalledForAResourceArea_ExpectTheAssociatedUriToBeReturned(string resourceAreaId, string url)
    {
        // Arrange
        var expectedUri = new Uri(url);

        var mockLocationService = new Mock<ILocationService>();
        mockLocationService.Setup(x => x.GetResourceAreaLocationAsync(resourceAreaId))
            .ReturnsAsync(url);

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ILocationService>(mockLocationService.Object);
        serviceCollection.AddResourceAreaUriResolver();

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        var resolver = serviceProvider.GetRequiredService<IResourceAreaUriResolver>();

        // Act
        var actualUri = resolver.Resolve(resourceAreaId);

        // Assert
        Assert.Equal(expectedUri, actualUri);
    }
}
