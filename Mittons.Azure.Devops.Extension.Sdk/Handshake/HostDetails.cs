using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record HostDetails : IHostDetails
{
    [JsonPropertyName("id")]
    public string? Id { get; }

    [JsonPropertyName("name")]
    public string? Name { get; }

    [JsonPropertyName("serviceVersion")]
    public string? ServiceVersion { get; }

    [JsonPropertyName("type")]
    public HostType? HostType { get; }
}
