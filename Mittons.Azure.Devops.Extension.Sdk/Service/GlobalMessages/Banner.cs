using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Service.GlobalMessages;

public class Banner
{
    /**
     * Buttons to display after the message
     */
    [JsonPropertyName("buttons")]
    public IEnumerable<Button>? Buttons { get; }

    /**
     * Custom icon name. Must be an icon in the Azure DevOps icon font.
     */
    [JsonPropertyName("customIcon")]
    public string? CustomIcon { get; }

    /**
     * Whether or not the message banner is dismissable. If false, do not show the close (X) icon.
     * @default true
     */
    [JsonPropertyName("dismissable")]
    public bool? Dismissable { get; }

    /**
     * Optional "?" icon to show after the message that has a tooltip with more information and/or a hyperlink.
     */
    [JsonPropertyName("helpInfo")]
    public HelpInfo? HelpInfo { get; }

    /**
     * banner level (controls the background and icon of the banner)
     */
    [JsonPropertyName("level")]
    public Severity? Serverity { get; }

    /**
     * Banner message. Ignored if messageFormat is also provided
     */
    [JsonPropertyName("message")]
    public string? Message { get; }

    /**
     * Banner message format string. Arguments (like \{0\}, \{1\} are filled in with hyperlinks supplied in messageLinks)
     */
    [JsonPropertyName("messageFormat")]
    public string? MessageFormat { get; }

    /**
     * Links to supply to the format arguments in `messageFormat`
     */
    [JsonPropertyName("messageLinks")]
    public IEnumerable<Link>? MessageLinks { get; }

    public Banner(
        IEnumerable<Button>? buttons,
        string? customIcon,
        bool? dismissable,
        HelpInfo? helpInfo,
        Severity? serverity,
        string? message,
        string? messageFormat,
        IEnumerable<Link>? messageLinks
    )
    {
        Buttons = buttons;
        CustomIcon = customIcon;
        Dismissable = dismissable;
        HelpInfo = helpInfo;
        Serverity = serverity;
        Message = message;
        MessageFormat = messageFormat;
        MessageLinks = messageLinks;
    }
}
