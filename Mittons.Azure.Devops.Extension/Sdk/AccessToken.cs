using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk;

public record AccessToken
{
    [JsonPropertyName("token")]
    public string? Token { get; init; }

    public AuthenticationHeaderValue AuthenticationHeader => new AuthenticationHeaderValue("Bearer", Token);
}
