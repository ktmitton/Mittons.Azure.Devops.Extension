using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record AccessToken([property: JsonPropertyName("token")] string? Token)
{
    public AuthenticationHeaderValue AuthenticationHeaderValue => new AuthenticationHeaderValue("Bearer", Token);
}
