using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Sdk;
using Mittons.Azure.Devops.Extension.Service;

namespace Mittons.Azure.Devops.Extension.Client;

internal static class IServiceCollectionRestClientExtensions
{
    // public static IServiceCollection AddRestClients(this IServiceCollection @serviceCollection)
    //     => @serviceCollection.AddGitClient();
}

public abstract class RestClient
{
    public virtual string? ResourceAreaId { get; } = default;

    protected string? RootPath { get; private set; }

    protected AuthenticationHeaderValue? AuthenticationHeader { get; private set; }

    protected readonly Task _ready;

    public RestClient(ISdk sdk, ILocationService locationService)
    {
        _ready = InitializeAsync(sdk, locationService);
    }

    protected virtual async Task InitializeAsync(ISdk sdk, ILocationService locationService)
    {
        await sdk.Ready;

        RootPath = ResourceAreaId is null ? await locationService.GetServiceLocationAsync(default, default) : await locationService.GetResourceAreaLocationAsync(ResourceAreaId);

        AuthenticationHeader = sdk.AuthenticationHeader;
    }

    protected Task<TReturn> SendRequestAsync<TBody, TReturn>(string apiVersion, string method, string route, Dictionary<string, string> queryParameters, TBody body)
    {
        //    var requestUrl = $"{RootPath}{route}?{string.Join("&", queryParameters.Select(x => $"{x.Key}={x.Value}"))}";
        throw new NotImplementedException();
    }

    protected Task<TReturn> SendRequestAsync<TReturn>(string apiVersion, string method, string route, Dictionary<string, string> queryParameters)
    {
        //    var requestUrl = $"{RootPath}{route}?{string.Join("&", queryParameters.Select(x => $"{x.Key}={x.Value}"))}";
        throw new NotImplementedException();
    }
}
