using System.Net.Http.Headers;
using System.Net.Http.Json;
using Mittons.Azure.Devops.Extension.Sdk;
using Mittons.Azure.Devops.Extension.Service;

namespace Mittons.Azure.Devops.Extension.Client;

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

    protected async Task<TReturn> SendRequestAsync<TBody, TReturn>(string apiVersion, HttpMethod method, string route, string acceptType, Dictionary<string, object?> queryParameters, TBody body)
    {
        var url = $"{RootPath}{route}?{string.Join("&", ConvertQueryParameters(queryParameters))}";

        var requestMessage = new HttpRequestMessage(method, url);
        requestMessage.Content = JsonContent.Create(body);

        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptType));
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"api-version={apiVersion}"));
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"excludeUrls=true"));
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"enumsAsNumbers=true"));
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"msDateFormat=true"));
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"noArrayWrap=true"));
        requestMessage.Headers.Add("X-VSS-ReauthenticationAction", "Suppress");
        requestMessage.Headers.Add("X-TFS-FedAuthRedirect", "Suppress");
        requestMessage.Headers.Authorization = AuthenticationHeader;

        var client = new HttpClient();
        var responseMessage = await client.SendAsync(requestMessage);

        return await responseMessage.Content.ReadFromJsonAsync<TReturn>();
    }

    protected async Task<TReturn> SendRequestAsync<TReturn>(string apiVersion, HttpMethod method, string route, string acceptType, Dictionary<string, object?> queryParameters)
    {
        var url = $"{RootPath}{route}?{string.Join("&", ConvertQueryParameters(queryParameters))}";

        var requestMessage = new HttpRequestMessage(method, url);

        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptType));
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"api-version={apiVersion}"));
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"excludeUrls=true"));
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"enumsAsNumbers=true"));
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"msDateFormat=true"));
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"noArrayWrap=true"));
        requestMessage.Headers.Add("X-VSS-ReauthenticationAction", "Suppress");
        requestMessage.Headers.Add("X-TFS-FedAuthRedirect", "Suppress");
        requestMessage.Headers.Authorization = AuthenticationHeader;

        var client = new HttpClient();
        var responseMessage = await client.SendAsync(requestMessage);

        return await responseMessage.Content.ReadFromJsonAsync<TReturn>();
    }

    private IEnumerable<string> ConvertQueryParameters(Dictionary<string, object?> queryParameters)
        => queryParameters.Where(x => !(x.Value is null)).SelectMany(x =>
        {
            if (x.Value is null)
            {
                return Enumerable.Empty<string>();
            }

            var type = x.Value.GetType();

            if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal))
            {
                return new[] { $"{x.Key}={x.Value}" };
            }
            else if (type.IsClass || type.IsValueType)
            {
                var innerProperties = new Dictionary<string, string>();

                return type.GetProperties().Select(y => $"{x.Key}.{y.Name}={y.GetValue(x.Value)}").ToArray();
            }

            return Enumerable.Empty<string>();
        });
}
