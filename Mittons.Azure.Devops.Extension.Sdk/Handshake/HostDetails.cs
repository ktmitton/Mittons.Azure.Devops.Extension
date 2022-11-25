using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record HostDetails(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("serviceVersion")] string? ServiceVersion,
    [property: JsonPropertyName("type")] HostType? HostType
) : IHostDetails;
