using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk;

public record InitializationRequest
{
    public const decimal DefaultSdkVersion = 3.0m;

    /// <summary>
    /// Indicates that the content of this extension is ready to be shown/used as soon as the
    /// initialization handshake has completed.
    /// </summary>
    /// <remarks>
    /// If this is set to false, the extension must call DevOps.notifyLoadSucceeded() once
    /// it has finished loading.
    /// </remarks>
    [JsonPropertyName("loaded")]
    public bool IsLoaded { get; }

    /// <summary>
    /// Extensions that show UI should specify this to true in order for the current user's theme
    /// to be applied to this extension content.
    /// </summary>
    [JsonPropertyName("applyTheme")]
    public bool ApplyTheme { get; }

    /// <summary>
    /// The version of the Azure Devops Extension Sdk being targeted.
    /// </summary>
    [JsonPropertyName("sdkVersion")]
    public decimal SdkVersion { get; }

    public InitializationRequest(decimal sdkVersion, bool isLoaded, bool applyTheme)
    {
        SdkVersion = sdkVersion;
        IsLoaded = isLoaded;
        ApplyTheme = applyTheme;
    }
}
