using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk;

internal record InitializationResponse
{
    [JsonPropertyName("contributionId")]
    public string? ContributionId { get; init; }

    [JsonPropertyName("context")]
    public Context? Context { get; init; }

    [JsonPropertyName("initialConfig")]
    public Dictionary<string, object>? InitialConfig { get; init; }

    [JsonPropertyName("themeData")]
    public Dictionary<string, string>? ThemeData { get; init; }
}