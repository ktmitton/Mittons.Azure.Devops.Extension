using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record ExtensionDetails(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("publisherId")] string? PublisherId,
    [property: JsonPropertyName("extensionId")] string? ExtensionId,
    [property: JsonPropertyName("version")] string? Version
) : IExtensionDetails;
