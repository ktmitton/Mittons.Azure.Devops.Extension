using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Mittons.Azure.Devops.Extension.Client;

internal class RestClient
{
    private readonly HttpClient _httpClient;

    public RestClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private MediaTypeWithQualityHeaderValue CreateAcceptHeader(string acceptType, string apiVersion)
    {
        var mediaType = new MediaTypeWithQualityHeaderValue(acceptType);
        mediaType.Parameters.Add(new NameValueHeaderValue("api-version", apiVersion));
        mediaType.Parameters.Add(new NameValueHeaderValue("excludeUrls", "true"));
        mediaType.Parameters.Add(new NameValueHeaderValue("enumsAsNumbers", "true"));
        mediaType.Parameters.Add(new NameValueHeaderValue("msDateFormat", "true"));
        mediaType.Parameters.Add(new NameValueHeaderValue("noArrayWrap", "true"));

        return mediaType;
    }

    protected async Task<TReturn> SendRequestAsync<TBody, TReturn>(string apiVersion, HttpMethod method, string route, string acceptType, Dictionary<string, object?> queryParameters, TBody body)
    {
        var url = $"{route}?{string.Join("&", ConvertQueryParameters(queryParameters))}";

        var requestMessage = new HttpRequestMessage(method, url);
        requestMessage.Content = JsonContent.Create(body);

        requestMessage.Headers.Accept.Add(CreateAcceptHeader(acceptType, apiVersion));

        var responseMessage = await _httpClient.SendAsync(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        var result = await responseMessage.Content.ReadFromJsonAsync<TReturn>();

        if (result is null)
        {
            throw new NullReferenceException("Null value returned from api");
        }

        return result;
    }

    protected async Task<TReturn> SendRequestAsync<TReturn>(string apiVersion, HttpMethod method, string route, string acceptType, Dictionary<string, object?> queryParameters)
    {
        var url = $"{route}?{string.Join("&", ConvertQueryParameters(queryParameters))}";
        System.Console.WriteLine(url);

        var requestMessage = new HttpRequestMessage(method, url);

        requestMessage.Headers.Accept.Add(CreateAcceptHeader(acceptType, apiVersion));

        var responseMessage = await _httpClient.SendAsync(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        var result = await responseMessage.Content.ReadFromJsonAsync<TReturn>();

        if (result is null)
        {
            throw new NullReferenceException("Null value returned from api");
        }

        return result;
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
