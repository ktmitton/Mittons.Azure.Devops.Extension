using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record ExtensionDetails : IExtensionDetails
{
    [JsonPropertyName("id")]
    public string? Id { get; }

    [JsonPropertyName("publisherId")]
    public string? PublisherId { get; }

    [JsonPropertyName("extensionId")]
    public string? ExtensionId { get; }

    [JsonPropertyName("version")]
    public string? Version { get; }
}
