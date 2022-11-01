using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record AccessToken
{
    [JsonPropertyName("token")]
    public string? Token { get; init; }

    public AuthenticationHeaderValue AuthenticationHeaderValue => new AuthenticationHeaderValue("Bearer", Token);
}
