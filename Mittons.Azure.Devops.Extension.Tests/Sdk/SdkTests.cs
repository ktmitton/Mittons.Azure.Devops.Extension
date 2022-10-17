using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Net.Http;
using Mittons.Azure.Devops.Extension.Sdk;
using Mittons.Azure.Devops.Extension.Xdm;

namespace Mittons.Azure.Devops.Extension.Tests.Sdk;

public class SdkTests : IDisposable
{
    private readonly ServiceCollection _serviceCollection = new();

    private readonly ServiceProvider _serviceProvider;

    private readonly Mock<IChannel> _mockChannel = new();

    private readonly Mock<IResourceAreaUriResolver> _mockResourceAreaUriResolver = new();

    public SdkTests()
    {
        _serviceCollection.AddSingleton<IChannel>(_mockChannel.Object);
        _serviceCollection.AddSingleton<IResourceAreaUriResolver>(_mockResourceAreaUriResolver.Object);
        _serviceCollection.AddSdk();

        _serviceProvider = _serviceCollection.BuildServiceProvider();

        _mockChannel.Setup(x => x.InitializeAsync(It.IsAny<decimal>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new InitializationResponse
            {
                ContributionId = default,
                Context = default
            });
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    [Theory]
    [InlineData(3.0, true, true, 3.0, true, true)]
    [InlineData(3.0, false, true, 3.0, false, true)]
    [InlineData(3.0, true, false, 3.0, true, false)]
    [InlineData(3.0, false, false, 3.0, false, false)]
    [InlineData(2.1, true, true, 2.1, true, true)]
    [InlineData(2.1, false, true, 2.1, false, true)]
    [InlineData(2.1, true, false, 2.1, true, false)]
    [InlineData(2.1, false, false, 2.1, false, false)]
    public async Task InitializeAsync_WhenInitializedWithValues_ExpectTheChannelToBeInitializedWithValues(
        decimal providedSdkVersion,
        bool providedIsLoaded,
        bool providedApplyTheme,
        decimal expectedSdkVersion,
        bool expectedIsLoaded,
        bool expectedApplyTheme
    )
    {
        // Arrange
        var sdk = _serviceProvider.GetRequiredService<ISdk>();
        var cancellationToken = new CancellationToken();

        // Act
        await sdk.InitializeAsync(providedSdkVersion, providedIsLoaded, providedApplyTheme, cancellationToken);

        // Assert
        _mockChannel.Verify(x => x.InitializeAsync(expectedSdkVersion, expectedIsLoaded, expectedApplyTheme, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_WhenInitializedWithNoValues_ExpectTheChannelToBeInitializedWithDefaultValues()
    {
        // Arrange
        var expectedSdkVersion = 3.0m;
        var expectedIsLoaded = true;
        var expectedApplyTheme = true;

        var sdk = _serviceProvider.GetRequiredService<ISdk>();
        var cancellationToken = new CancellationToken();

        // Act
        await sdk.InitializeAsync(cancellationToken: cancellationToken);

        // Assert
        _mockChannel.Verify(x => x.InitializeAsync(expectedSdkVersion, expectedIsLoaded, expectedApplyTheme, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_WhenInitializedWithDefaults_ExpectTheResourceAreaUriResolverToBePrimed()
    {
        // Arrange
        var sdk = _serviceProvider.GetRequiredService<ISdk>();
        var cancellationToken = new CancellationToken();

        // Act
        await sdk.InitializeAsync(cancellationToken: cancellationToken);

        // Assert
        _mockResourceAreaUriResolver.Verify(x => x.PrimeKnownResourceAreasAsync(cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData("Test1")]
    public async Task InitializeAsync_WhenInitialized_ExpectContextParametersToBeSet(string expectedContributionId)
    {
        // Arrange
        var sdk = _serviceProvider.GetRequiredService<ISdk>();
        var cancellationToken = new CancellationToken();

        var expectedContext = new Context();

        _mockChannel.Reset();
        _mockChannel.Setup(x => x.InitializeAsync(It.IsAny<decimal>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new InitializationResponse
            {
                ContributionId = expectedContributionId,
                Context = expectedContext
            });

        // Act
        await sdk.InitializeAsync(cancellationToken: cancellationToken);

        // Assert
        Assert.Equal(expectedContributionId, sdk.ContributionId);
        Assert.Equal(expectedContext, sdk.Context);
    }
}
