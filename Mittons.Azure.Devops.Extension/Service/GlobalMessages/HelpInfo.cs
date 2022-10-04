using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Service.GlobalMessages;

public class HelpInfo
{
    /**
     * If supplied the help icon will act as a hyperlink to the specified target href
     */
    [JsonPropertyName("href")]
    public string? Href { get; }

    /**
     * If supplied, hovering/focusing the help icon will show the given tooltip text
     */
    [JsonPropertyName("tooltip")]
    public string? Tooltip { get; }

    public HelpInfo(string? href, string? tooltip)
    {
        Href = href;
        Tooltip = tooltip;
    }
}