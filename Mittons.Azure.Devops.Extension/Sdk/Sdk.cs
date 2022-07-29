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

        @serviceCollection.AddRestClients();

        @serviceCollection.AddSingleton<ISdk, Sdk>();

        return @serviceCollection;
    }
}

// https://github.com/microsoft/azure-devops-extension-sdk/blob/5d6b4c09f33c2adb55afeeb57d1047a53cc76cf8/src/SDK.ts
public interface ISdk
{
    string? ContributionId { get; }

    Context? Context { get; }

    Dictionary<string, string>? ThemeData { get; }

    AuthenticationHeaderValue? AuthenticationHeader { get; }

    Task Ready { get; }

    Task InitializeAsync(decimal sdkVersion = InitializationRequest.DefaultSdkVersion, bool isLoaded = true, bool applyTheme = true);
}

internal class Sdk : ISdk
{
    private readonly IChannel _channel;

    public string? ContributionId { get; private set; }

    public Context? Context { get; private set; }

    public Dictionary<string, string>? ThemeData { get; private set; }

    private TaskCompletionSource _readyCompletionSource = new TaskCompletionSource();

    public Task Ready => _readyCompletionSource.Task;

    public AuthenticationHeaderValue? AuthenticationHeader { get; private set; }

    public Sdk(IChannel channel)
    {
        _channel = channel;
    }

    public async Task InitializeAsync(decimal sdkVersion = InitializationRequest.DefaultSdkVersion, bool isLoaded = true, bool applyTheme = true)
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

        _readyCompletionSource.SetResult();

        System.Console.WriteLine("Initialization Complete");
    }
}
