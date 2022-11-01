using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Sdk.Handshake;
using Mittons.Azure.Devops.Extension.Sdk.Xdm;

namespace Mittons.Azure.Devops.Extension.Sdk;

public static class IServiceCollectionSdkExtensions
{
    public static IServiceCollection AddAzureDevopsExtensionSdk(this IServiceCollection @serviceCollection)
    {
        @serviceCollection.AddXdmChannel();

        // @serviceCollection.AddExtensionDataService();
        // @serviceCollection.AddGlobalMessagesService();
        // @serviceCollection.AddHostNavigationService();
        // @serviceCollection.AddHostPageLayoutService();
        // @serviceCollection.AddLocationService();
        // @serviceCollection.AddProjectPageService();
        // @serviceCollection.AddAccountsClient();
        //@serviceCollection.AddGitClient();
        //@serviceCollection.AddTestClient();

        //@serviceCollection.AddRestClients();

        @serviceCollection.AddSdk();

        return @serviceCollection;
    }

    public static IServiceCollection AddSdk(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<ISdk, Sdk>();
}

internal class Sdk : ISdk
{
    private readonly IChannel _channel;

    private readonly IResourceAreaUriResolver _resourceAreaUriResolver;

    public string? ContributionId { get; private set; }

    public Dictionary<string, string>? ThemeData { get; private set; }

    public Dictionary<string, object>? InitialConfiguration { get; private set; }

    public IExtensionDetails? ExtensionDetails { get; private set; }

    public IUserDetails? UserDetails { get; private set; }

    public IHostDetails? HostDetails { get; private set; }

    public AuthenticationHeaderValue? AuthenticationHeaderValue { get; private set; }

    public Sdk(IChannel channel, IResourceAreaUriResolver resourceAreaUriResolver)
    {
        _channel = channel;
        _resourceAreaUriResolver = resourceAreaUriResolver;
    }

    public async Task InitializeAsync(decimal sdkVersion = 3.0m, bool isLoaded = true, bool applyTheme = true, CancellationToken cancellationToken = default)
    {
        await _channel.InitializeAsync(cancellationToken);

        await PerformInitialHandshake(sdkVersion, isLoaded, applyTheme, cancellationToken);

        await GetAuthenticationHeaderAsync(cancellationToken);

        await _resourceAreaUriResolver.PrimeKnownResourceAreasAsync(cancellationToken);

        System.Console.WriteLine("Initialization Complete");
    }

    private async Task PerformInitialHandshake(decimal sdkVersion, bool isLoaded, bool applyTheme, CancellationToken cancellationToken)
    {
        var initOptions = new InitializationRequest(
            sdkVersion: sdkVersion,
            isLoaded: isLoaded,
            applyTheme: applyTheme
        );

        var result = await _channel.InvokeRemoteMethodAsync<InitializationResponse>("initialHandshake", InstanceId.HostControl, cancellationToken, initOptions);

        ContributionId = result.ContributionId;
        ExtensionDetails = result.Context?.ExtensionDetails;
        UserDetails = result.Context?.UserDetails;
        HostDetails = result.Context?.HostDetails;
        InitialConfiguration = result.InitialConfiguration;
        ThemeData = result.ThemeData;
    }

    private async Task GetAuthenticationHeaderAsync(CancellationToken cancellationToken)
    {
        var accessToken = await _channel.InvokeRemoteMethodAsync<AccessToken>("getAccessToken", InstanceId.HostControl, cancellationToken);

        AuthenticationHeaderValue = accessToken.AuthenticationHeaderValue;
    }
}
