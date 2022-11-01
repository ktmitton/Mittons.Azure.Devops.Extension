using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record Context
{
    [JsonPropertyName("extension")]
    public ExtensionDetails? ExtensionDetails { get; }

    [JsonPropertyName("user")]
    public UserDetails? UserDetails { get; }

    [JsonPropertyName("host")]
    public HostDetails? HostDetails { get; }
}
