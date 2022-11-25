using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record Context(
    [property: JsonPropertyName("extension")] ExtensionDetails? ExtensionDetails,
    [property: JsonPropertyName("user")] UserDetails? UserDetails,
    [property: JsonPropertyName("host")] HostDetails? HostDetails
);
