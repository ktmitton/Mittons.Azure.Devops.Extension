using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Sdk;
using Mittons.Azure.Devops.Extension.Sdk.Handshake;
using Mittons.Azure.Devops.Extension.Sdk.Xdm;

namespace Mittons.Azure.Devops.Extension.Tests.Sdk;

public class SdkTests : IDisposable
{
    private readonly ServiceCollection _serviceCollection = new();

    private readonly ServiceProvider _serviceProvider;

    private readonly Mock<IChannel> _mockChannel = new();

    private readonly Mock<IResourceAreaUriResolver> _mockResourceAreaUriResolver = new();

    private InitializationResponse _initializationResponse = new InitializationResponse(
        ContributionId: Guid.NewGuid().ToString(),
        Context: new Context(default, default, default),
        InitialConfiguration: new(),
        ThemeData: new()
    );

    private AccessToken _accessToken = new AccessToken(Guid.NewGuid().ToString());

    public SdkTests()
    {
        _serviceCollection.AddSingleton<IChannel>(_mockChannel.Object);
        _serviceCollection.AddSingleton<IResourceAreaUriResolver>(_mockResourceAreaUriResolver.Object);
        _serviceCollection.AddSdk();

        _serviceProvider = _serviceCollection.BuildServiceProvider();

        _mockChannel.Setup(x => x.InvokeRemoteMethodAsync<InitializationResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<InitializationRequest>()))
            .ReturnsAsync(_initializationResponse);

        _mockChannel.Setup(x => x.InvokeRemoteMethodAsync<AccessToken>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_accessToken);
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    [Fact]
    public async Task InitializeAsync_WhenInitializedWithNoValues_ExpectTheChannelToBeInitialized()
    {
        // Arrange
        var sdk = _serviceProvider.GetRequiredService<ISdk>();
        var cancellationToken = new CancellationToken();

        // Act
        await sdk.InitializeAsync(cancellationToken: cancellationToken);

        // Assert
        _mockChannel.Verify(x => x.InitializeAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_WhenInitializedWithDefaults_ExpectTheResourceAreaUriResolverToBePrimed()
    {
        // Arrange
        var sdk = _serviceProvider.GetRequiredService<ISdk>();
        var expectedCancellationToken = new CancellationTokenSource().Token;

        // Act
        await sdk.InitializeAsync(cancellationToken: expectedCancellationToken);

        // Assert
        _mockResourceAreaUriResolver.Verify(x => x.PrimeKnownResourceAreasAsync(expectedCancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(1.0, true, true)]
    [InlineData(2.0, true, false)]
    [InlineData(3.0, false, true)]
    [InlineData(4.0, false, false)]
    public async Task InitializeAsync_WhenInitializedWithSettings_ExpectAHandshakeToBePerformed(decimal expectedSdkVersion, bool expectedIsLoaded, bool expectedApplyTheme)
    {
        // Arrange
        var sdk = _serviceProvider.GetRequiredService<ISdk>();
        var expectedCancellationToken = new CancellationTokenSource().Token;

        var expectedMethodName = "initialHandshake";
        var expectedInstanceId = InstanceId.HostControl;
        var expectedInitializationRequest = new InitializationRequest(
            SdkVersion: expectedSdkVersion,
            IsLoaded: expectedIsLoaded,
            ApplyTheme: expectedApplyTheme
        );

        // Act
        await sdk.InitializeAsync(expectedSdkVersion, expectedIsLoaded, expectedApplyTheme, expectedCancellationToken);

        // Assert
        _mockChannel.Verify(x => x.InvokeRemoteMethodAsync<InitializationResponse>(expectedMethodName, expectedInstanceId, expectedCancellationToken, expectedInitializationRequest), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_WhenInitializedWithNoSettings_ExpectAHandshakeToBePerformed()
    {
        // Arrange
        var sdk = _serviceProvider.GetRequiredService<ISdk>();
        var expectedCancellationToken = new CancellationTokenSource().Token;

        var expectedMethodName = "initialHandshake";
        var expectedInstanceId = InstanceId.HostControl;
        var expectedInitializationRequest = new InitializationRequest(
            SdkVersion: 3.0m,
            IsLoaded: true,
            ApplyTheme: true
        );

        // Act
        await sdk.InitializeAsync(cancellationToken: expectedCancellationToken);

        // Assert
        _mockChannel.Verify(x => x.InvokeRemoteMethodAsync<It.IsAnyType>(expectedMethodName, expectedInstanceId, expectedCancellationToken, expectedInitializationRequest), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_WhenInitialized_ExpectContextParametersToBeSet()
    {
        // Arrange
        var sdk = _serviceProvider.GetRequiredService<ISdk>();

        // Act
        await sdk.InitializeAsync();

        // Assert
        Assert.Equal(_initializationResponse.ContributionId, sdk.ContributionId);
        Assert.Equal(_initializationResponse.Context?.ExtensionDetails, sdk.ExtensionDetails);
        Assert.Equal(_initializationResponse.Context?.UserDetails, sdk.UserDetails);
        Assert.Equal(_initializationResponse.Context?.HostDetails, sdk.HostDetails);
        Assert.Equal(_initializationResponse.InitialConfiguration, sdk.InitialConfiguration);
        Assert.Equal(_initializationResponse.ThemeData, sdk.ThemeData);
    }

    [Fact]
    public async Task InitializeAsync_WhenInitialized_ExpectARequestForAuthenticationSettings()
    {
        // Arrange
        var sdk = _serviceProvider.GetRequiredService<ISdk>();
        var expectedCancellationToken = new CancellationTokenSource().Token;

        var expectedMethodName = "getAccessToken";
        var expectedInstanceId = InstanceId.HostControl;

        // Act
        await sdk.InitializeAsync(cancellationToken: expectedCancellationToken);

        // Assert
        _mockChannel.Verify(x => x.InvokeRemoteMethodAsync<AccessToken>(expectedMethodName, expectedInstanceId, expectedCancellationToken), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_WhenInitialized_ExpectAuthenticationHeaderToBeSet()
    {
        // Arrange
        var sdk = _serviceProvider.GetRequiredService<ISdk>();

        // Act
        await sdk.InitializeAsync();

        // Assert
        Assert.Equal(_accessToken.AuthenticationHeaderValue, sdk.AuthenticationHeaderValue);
    }
}
