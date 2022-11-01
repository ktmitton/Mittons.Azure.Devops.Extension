using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Handshake;

internal record InitializationRequest
{
    [JsonPropertyName("loaded")]
    public bool IsLoaded { get; }

    [JsonPropertyName("applyTheme")]
    public bool ApplyTheme { get; }

    [JsonPropertyName("sdkVersion")]
    public decimal SdkVersion { get; }

    public InitializationRequest(decimal sdkVersion, bool isLoaded, bool applyTheme)
    {
        SdkVersion = sdkVersion;
        IsLoaded = isLoaded;
        ApplyTheme = applyTheme;
    }
}
