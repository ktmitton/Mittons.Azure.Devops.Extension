using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record UserDetails : IUserDetails
{
    [JsonPropertyName("descriptor")]
    public string? Descriptor { get; }

    [JsonPropertyName("id")]
    public string? Id { get; }

    [JsonPropertyName("name")]
    public string? Name { get; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; }

    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; }
}
