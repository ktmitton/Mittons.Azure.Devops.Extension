using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Client;
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
        var actualUri = await resolver.ResolveAsync(resourceAreaId, default);

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

    [Theory]
    [InlineData(default(string), "https://localhost/first")]
    [InlineData(default(string), "https://localhost/second")]
    [InlineData("", "https://localhost/first")]
    [InlineData("", "https://localhost/second")]
    [InlineData(" ", "https://localhost/first")]
    [InlineData(" ", "https://localhost/second")]
    public async Task ResolveAsync_WhenCalledForNoResourceArea_ExpectTheServiceLocationToBeReturned(string? resoureceAreaId, string url)
    {
        // Arrange
        var expectedUri = new Uri(url);

        var mockLocationService = new Mock<ILocationService>();
        mockLocationService.Setup(x => x.GetServiceLocationAsync(default, default))
            .ReturnsAsync(url);

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ILocationService>(mockLocationService.Object);
        serviceCollection.AddResourceAreaUriResolver();

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        var resolver = serviceProvider.GetRequiredService<IResourceAreaUriResolver>();

        // Act
        var actualUri = await resolver.ResolveAsync(resoureceAreaId, default);

        // Assert
        Assert.Equal(expectedUri, actualUri);
    }

    [Theory]
    [InlineData(default(string), "https://localhost/first")]
    [InlineData(default(string), "https://localhost/second")]
    [InlineData("", "https://localhost/first")]
    [InlineData("", "https://localhost/second")]
    [InlineData(" ", "https://localhost/first")]
    [InlineData(" ", "https://localhost/second")]
    public void Resolve_WhenCalledForNoResourceArea_ExpectTheServiceLocationToBeReturned(string? resoureceAreaId, string url)
    {
        // Arrange
        var expectedUri = new Uri(url);

        var mockLocationService = new Mock<ILocationService>();
        mockLocationService.Setup(x => x.GetServiceLocationAsync(default, default))
            .ReturnsAsync(url);

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ILocationService>(mockLocationService.Object);
        serviceCollection.AddResourceAreaUriResolver();

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        var resolver = serviceProvider.GetRequiredService<IResourceAreaUriResolver>();

        // Act
        var actualUri = resolver.Resolve(resoureceAreaId);

        // Assert
        Assert.Equal(expectedUri, actualUri);
    }

    [Theory]
    [InlineData(ResourceAreaId.Git)]
    [InlineData(ResourceAreaId.Accounts)]
    [InlineData(ResourceAreaId.Boards)]
    [InlineData(ResourceAreaId.Builds)]
    [InlineData(ResourceAreaId.Core)]
    [InlineData(ResourceAreaId.Dashboard)]
    [InlineData(ResourceAreaId.ExtensionManagement)]
    [InlineData(ResourceAreaId.Git)]
    [InlineData(ResourceAreaId.Graph)]
    [InlineData(ResourceAreaId.Policy)]
    [InlineData(ResourceAreaId.ProjectAnalysis)]
    [InlineData(ResourceAreaId.Release)]
    [InlineData(ResourceAreaId.ServiceEndpoint)]
    [InlineData(ResourceAreaId.TaskAgent)]
    [InlineData(ResourceAreaId.Test)]
    [InlineData(ResourceAreaId.Tfvc)]
    [InlineData(ResourceAreaId.Wiki)]
    [InlineData(ResourceAreaId.Work)]
    [InlineData(ResourceAreaId.WorkItemTracking)]
    public async Task PrimeKnownResourceAreasAsync_WhenPrimed_ExpectResoureceAreaUriToBeLoaded(string resourceAreaId)
    {
        // Arrange
        var mockLocationService = new Mock<ILocationService>();
        mockLocationService.Setup(x => x.GetResourceAreaLocationAsync(It.IsAny<string>()))
            .ReturnsAsync("https://localhost");
        mockLocationService.Setup(x => x.GetServiceLocationAsync(default, default))
            .ReturnsAsync("https://localhost");

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ILocationService>(mockLocationService.Object);
        serviceCollection.AddResourceAreaUriResolver();

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        var resolver = serviceProvider.GetRequiredService<IResourceAreaUriResolver>();

        // Act
        await resolver.PrimeKnownResourceAreasAsync(default);

        // Assert
        mockLocationService.Verify(x => x.GetResourceAreaLocationAsync(resourceAreaId), Times.Once);
    }

    [Fact]
    public async Task PrimeKnownResourceAreasAsync_WhenPrimed_ExpectServiceLocationToBeLoaded()
    {
        // Arrange
        var mockLocationService = new Mock<ILocationService>();
        mockLocationService.Setup(x => x.GetResourceAreaLocationAsync(It.IsAny<string>()))
            .ReturnsAsync("https://localhost");
        mockLocationService.Setup(x => x.GetServiceLocationAsync(default, default))
            .ReturnsAsync("https://localhost");

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ILocationService>(mockLocationService.Object);
        serviceCollection.AddResourceAreaUriResolver();

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        var resolver = serviceProvider.GetRequiredService<IResourceAreaUriResolver>();

        // Act
        await resolver.PrimeKnownResourceAreasAsync(default);

        // Assert
        mockLocationService.Verify(x => x.GetServiceLocationAsync(default, default), Times.Once);
    }
}
