using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Client;
using Mittons.Azure.Devops.Extension.Api.Net.Http;
using Mittons.Azure.Devops.Extension.Service.ExtensionData;
using Mittons.Azure.Devops.Extension.Service.GlobalMessages;
using Mittons.Azure.Devops.Extension.Service.HostNavigation;
using Mittons.Azure.Devops.Extension.Service.HostPageLayout;
using Mittons.Azure.Devops.Extension.Service.Location;
using Mittons.Azure.Devops.Extension.Service.ProjectPage;
using Mittons.Azure.Devops.Extension.Xdm;

namespace Mittons.Azure.Devops.Extension.Sdk;

public static class IServiceCollectionSdkExtensions
{
    public static IServiceCollection AddAzureDevopsExtensionSdk(this IServiceCollection @serviceCollection)
    {
        @serviceCollection.AddXdmChannel();

        @serviceCollection.AddExtensionDataService();
        @serviceCollection.AddGlobalMessagesService();
        @serviceCollection.AddHostNavigationService();
        @serviceCollection.AddHostPageLayoutService();
        @serviceCollection.AddLocationService();
        @serviceCollection.AddProjectPageService();
        @serviceCollection.AddAccountsClient();
        //@serviceCollection.AddGitClient();
        //@serviceCollection.AddTestClient();

        //@serviceCollection.AddRestClients();

        @serviceCollection.AddSdk();

        return @serviceCollection;
    }

    public static IServiceCollection AddSdk(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<ISdk, Sdk>();
}

// https://github.com/microsoft/azure-devops-extension-sdk/blob/5d6b4c09f33c2adb55afeeb57d1047a53cc76cf8/src/SDK.ts
public interface ISdk
{
    string? ContributionId { get; }

    Context? Context { get; }

    Dictionary<string, string>? ThemeData { get; }

    Dictionary<string, object>? InitialConfiguration { get; }

    AuthenticationHeaderValue? AuthenticationHeader { get; }

    Task InitializeAsync(decimal sdkVersion = InitializationRequest.DefaultSdkVersion, bool isLoaded = true, bool applyTheme = true, CancellationToken cancellationToken = default);
}

internal class Sdk : ISdk
{
    private readonly IChannel _channel;

    private readonly IResourceAreaUriResolver _resourceAreaUriResolver;

    public string? ContributionId { get; private set; }

    public Context? Context { get; private set; }

    public Dictionary<string, string>? ThemeData { get; private set; }

    public Dictionary<string, object>? InitialConfiguration { get; private set; }

    public AuthenticationHeaderValue? AuthenticationHeader { get; private set; }

    public Sdk(IChannel channel, IResourceAreaUriResolver resourceAreaUriResolver)
    {
        _channel = channel;
        _resourceAreaUriResolver = resourceAreaUriResolver;
    }

    public async Task InitializeAsync(decimal sdkVersion = InitializationRequest.DefaultSdkVersion, bool isLoaded = true, bool applyTheme = true, CancellationToken cancellationToken = default)
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
        Context = result.Context;
        InitialConfiguration = result.InitialConfiguration;
        ThemeData = result.ThemeData;
    }

    private async Task GetAuthenticationHeaderAsync(CancellationToken cancellationToken)
    {
        var accessToken = await _channel.InvokeRemoteMethodAsync<AccessToken>("getAccessToken", InstanceId.HostControl, cancellationToken);

        AuthenticationHeader = accessToken.AuthenticationHeader;
    }
}
