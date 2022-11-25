using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record InitializationRequest(
    [property: JsonPropertyName("loaded")] bool IsLoaded,
    [property: JsonPropertyName("applyTheme")] bool ApplyTheme,
    [property: JsonPropertyName("sdkVersion")] decimal SdkVersion
);
