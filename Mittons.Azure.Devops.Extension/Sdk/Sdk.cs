using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Client;
using Mittons.Azure.Devops.Extension.Service;
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
        @serviceCollection.AddGitClient();

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

    AuthenticationHeaderValue? AuthenticationHeader { get; }

    Task InitializeAsync(decimal sdkVersion = InitializationRequest.DefaultSdkVersion, bool isLoaded = true, bool applyTheme = true, CancellationToken cancellationToken = default);

    Task NotifyLoadSucceededAsync(CancellationToken cancellationToken = default);
}

internal class Sdk : ISdk
{
    private readonly IChannel _channel;

    public string? ContributionId { get; private set; }

    public Context? Context { get; private set; }

    public Dictionary<string, string>? ThemeData { get; private set; }

    public AuthenticationHeaderValue? AuthenticationHeader { get; private set; }

    public Sdk(IChannel channel)
    {
        _channel = channel;
    }

    public async Task InitializeAsync(decimal sdkVersion = InitializationRequest.DefaultSdkVersion, bool isLoaded = true, bool applyTheme = true, CancellationToken cancellationToken = default)
    {
        var initOptions = new InitializationRequest(
            sdkVersion: sdkVersion,
            isLoaded: isLoaded,
            applyTheme: applyTheme
        );

        var result = await _channel.InvokeRemoteMethodAsync<InitializationResponse>("initialHandshake", InstanceId.HostControl, initOptions);

        ContributionId = result.ContributionId;
        Context = result.Context;

        var accessToken = await _channel.InvokeRemoteMethodAsync<AccessToken>("getAccessToken", InstanceId.HostControl);

        AuthenticationHeader = accessToken.AuthenticationHeader;

        System.Console.WriteLine("Initialization Complete");
    }

    public async Task NotifyLoadSucceededAsync(CancellationToken cancellationToken = default)
        => await _channel.InvokeRemoteMethodAsync("notifyLoadSucceeded", InstanceId.HostControl);
}
