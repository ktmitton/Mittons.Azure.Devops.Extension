using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Service.GlobalMessages;

public class Button
{
    /**
     * The id of a command contribution to execute when the button is clicked
     */
    [JsonPropertyName("command")]
    public string? Command { get; }

    /**
     * Optional arugments to pass when invoking the supplied command
     */
    [JsonPropertyName("commandArguments")]
    public IEnumerable<object>? CommandArguments { get; }

    /**
     * Id of the contribution that the button was defined in (optional, used to resolve relative icon URLs)
     */
    [JsonPropertyName("contributionId")]
    public string? ContributionId { get; }

    /**
     * If true, the button cannot be interacted with.
     */
    [JsonPropertyName("disabled")]
    public bool? Disabled { get; }

    /**
     * Href to navigate to when the button is clicked.  Pass in if this is a link button.
     */
    [JsonPropertyName("href")]
    public string? Href { get; }

    /**
     * Either a url (relative or fully qualified) or an IContributedIconDefinition with
     * urls for light and dark themes. This allows the caller to use different styles of
     * icons based on the theme type.
     */
    [JsonPropertyName("icon")]
    public IconDefinition? Icon { get; }

    /**
     * Set to true if this button should be styled with a primary color.
     */
    [JsonPropertyName("primary")]
    public bool? Primary { get; }

    /**
     * Optional,context in which the linked resource will open.
     */
    [JsonPropertyName("target")]
    public string? Target { get; }

    /**
     * Text to render inside the button.
     */
    [JsonPropertyName("text")]
    public string? Text { get; }

    /**
     * Optional value to use as an aria-label and tooltip for the button.
     */
    [JsonPropertyName("tooltip")]
    public string? Tooltip { get; }

    public Button(
        string? command,
        IEnumerable<object>? commandArguments,
        string? contributionId,
        bool? disabled,
        string? href,
        IconDefinition? icon,
        bool? primary,
        string? target,
        string? text,
        string? tooltip
    )
    {
        Command = command;
        CommandArguments = commandArguments;
        ContributionId = contributionId;
        Disabled = disabled;
        Href = href;
        Icon = icon;
        Primary = primary;
        Target = target;
        Text = text;
        Tooltip = tooltip;
    }
}
