using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Service.GlobalMessages;

public class IconDefinition
{
    /**
     * Icon property (absolute url or relative asset path) to use when the current theme is a "light" theme.
     */
    [JsonPropertyName("light")]
    public string Light { get; }

    /**
     * Icon property (absolute url or relative asset path) to use when the current theme is a "dark" theme.
     */
    [JsonPropertyName("dark")]
    public string Dark { get; }

    public IconDefinition(string light, string dark)
    {
        Light = light;
        Dark = dark;
    }
}
