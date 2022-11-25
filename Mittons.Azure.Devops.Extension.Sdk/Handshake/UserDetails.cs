using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record UserDetails(
    [property: JsonPropertyName("descriptor")] string? Descriptor,
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("displayName")] string? DisplayName,
    [property: JsonPropertyName("imageUrl")] string? ImageUrl
) : IUserDetails;
