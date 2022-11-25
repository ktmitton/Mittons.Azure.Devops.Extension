using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record InitializationResponse(
    [property: JsonPropertyName("contributionId")] string? ContributionId,
    [property: JsonPropertyName("context")] Context? Context,
    [property: JsonPropertyName("initialConfig")] Dictionary<string, object>? InitialConfiguration,
    [property: JsonPropertyName("themeData")] Dictionary<string, string>? ThemeData
);
