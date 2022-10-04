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

    Dictionary<string, Uri>? ResourceAreaUris { get; }

    Task InitializeAsync(decimal sdkVersion = InitializationRequest.DefaultSdkVersion, bool isLoaded = true, bool applyTheme = true, CancellationToken cancellationToken = default);

    Task NotifyLoadSucceededAsync(CancellationToken cancellationToken = default);
}

internal class Sdk : ISdk
{
    private readonly IChannel _channel;

    private readonly ILocationService _locationService;

    public string? ContributionId { get; private set; }

    public Context? Context { get; private set; }

    public Dictionary<string, string>? ThemeData { get; private set; }

    public AuthenticationHeaderValue? AuthenticationHeader { get; private set; }

    public Dictionary<string, Uri>? ResourceAreaUris { get; private set; }

    public Sdk(IChannel channel, ILocationService locationService)
    {
        _channel = channel;
        _locationService = locationService;
    }

    public async Task InitializeAsync(decimal sdkVersion = InitializationRequest.DefaultSdkVersion, bool isLoaded = true, bool applyTheme = true, CancellationToken cancellationToken = default)
    {
        await _channel.InitializeAsync(cancellationToken);

        await PerformHandshakeAsync(sdkVersion, isLoaded, applyTheme, cancellationToken);

        await SetAuthenticationHeaderAsync(cancellationToken);

        await SetResourceAreaUris(cancellationToken);

        System.Console.WriteLine("Initialization Complete");
    }

    private async Task PerformHandshakeAsync(decimal sdkVersion = InitializationRequest.DefaultSdkVersion, bool isLoaded = true, bool applyTheme = true, CancellationToken cancellationToken = default)
    {
        var initOptions = new InitializationRequest(
            sdkVersion: sdkVersion,
            isLoaded: isLoaded,
            applyTheme: applyTheme
        );

        var result = await _channel.InvokeRemoteMethodAsync<InitializationResponse>("initialHandshake", InstanceId.HostControl, cancellationToken, initOptions);

        ContributionId = result.ContributionId;
        Context = result.Context;
    }

    private async Task SetAuthenticationHeaderAsync(CancellationToken cancellationToken = default)
    {
        var accessToken = await _channel.InvokeRemoteMethodAsync<AccessToken>("getAccessToken", InstanceId.HostControl, cancellationToken);

        AuthenticationHeader = accessToken.AuthenticationHeader;
    }

    private async Task SetResourceAreaUris(CancellationToken cancellationToken = default)
    {
        var resourceTasks = new Dictionary<string, Task<string>>
        {
            { ResourceAreaId.None, _locationService.GetServiceLocationAsync(default, default) },
            { ResourceAreaId.Accounts, GetResourceAreaLocationAsync(ResourceAreaId.Accounts) },
            { ResourceAreaId.Boards, GetResourceAreaLocationAsync(ResourceAreaId.Boards) },
            { ResourceAreaId.Builds, GetResourceAreaLocationAsync(ResourceAreaId.Builds) },
            { ResourceAreaId.Contributions, GetResourceAreaLocationAsync(ResourceAreaId.Contributions) },
            { ResourceAreaId.Core, GetResourceAreaLocationAsync(ResourceAreaId.Core) },
            { ResourceAreaId.Dashboard, GetResourceAreaLocationAsync(ResourceAreaId.Dashboard) },
            { ResourceAreaId.ExtensionManagement, GetResourceAreaLocationAsync(ResourceAreaId.ExtensionManagement) },
            { ResourceAreaId.Gallery, GetResourceAreaLocationAsync(ResourceAreaId.Gallery) },
            { ResourceAreaId.Git, GetResourceAreaLocationAsync(ResourceAreaId.Git) },
            { ResourceAreaId.Graph, GetResourceAreaLocationAsync(ResourceAreaId.Graph) },
            { ResourceAreaId.Policy, GetResourceAreaLocationAsync(ResourceAreaId.Policy) },
            { ResourceAreaId.Profile, GetResourceAreaLocationAsync(ResourceAreaId.Profile) },
            { ResourceAreaId.ProjectAnalysis, GetResourceAreaLocationAsync(ResourceAreaId.ProjectAnalysis) },
            { ResourceAreaId.Release, GetResourceAreaLocationAsync(ResourceAreaId.Release) },
            { ResourceAreaId.ServiceEndpoint, GetResourceAreaLocationAsync(ResourceAreaId.ServiceEndpoint) },
            { ResourceAreaId.TaskAgent, GetResourceAreaLocationAsync(ResourceAreaId.TaskAgent) },
            { ResourceAreaId.Test, GetResourceAreaLocationAsync(ResourceAreaId.Test) },
            { ResourceAreaId.Tfvc, GetResourceAreaLocationAsync(ResourceAreaId.Tfvc) },
            { ResourceAreaId.Wiki, GetResourceAreaLocationAsync(ResourceAreaId.Wiki) },
            { ResourceAreaId.Work, GetResourceAreaLocationAsync(ResourceAreaId.Work) },
            { ResourceAreaId.WorkItemTracking, GetResourceAreaLocationAsync(ResourceAreaId.WorkItemTracking) }
        };

        await Task.WhenAll(resourceTasks.Select(x => x.Value));

        ResourceAreaUris = new Dictionary<string, Uri>(
            resourceTasks.Where(x => x.Value.Result != string.Empty).Select(x => new KeyValuePair<string, Uri>(x.Key, new Uri(x.Value.Result)))
        );
    }

    private async Task<string> GetResourceAreaLocationAsync(string resourceAreaId)
    {
        try
        {
            return await _locationService.GetResourceAreaLocationAsync(resourceAreaId);
        }
        catch
        {
            return string.Empty;
        }
    }

    public async Task NotifyLoadSucceededAsync(CancellationToken cancellationToken = default)
        => await _channel.InvokeRemoteMethodAsync("notifyLoadSucceeded", InstanceId.HostControl, cancellationToken);
}
