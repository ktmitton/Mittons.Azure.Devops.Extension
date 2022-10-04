using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Service.GlobalMessages;

public class Dialog
{
    /**
     * Props to pass to buttons in the footer.
     */
    [JsonPropertyName("buttonProps")]
    public IEnumerable<object> ButtonProps { get; }

    /**
     * Props to pass to the close button.
     */
    [JsonPropertyName("closeButtonProps")]
    public object? CloseButtonProps { get; }

    /**
     * Props (type: ICustomDialogProps) to pass to the custom contribution.
     */
    [JsonPropertyName("contentProperties")]
    public object? ContentProperties { get; }

    /**
     * The id of the custom contribution to host in the dialog. Used to render a customized dialog.
     */
    [JsonPropertyName("contributionId")]
    public string? ContributionId { get; }

    /**
     * Optional initial configuration for the contributed content. Used in pair with contributionId.
     */
    [JsonPropertyName("contributionConfiguration")]
    public object? ContributionConfiguration { get; }

    /**
     * The text of the dialog message. Ignored if contributionId is specified.
     */
    [JsonPropertyName("message")]
    public string? Message { get; }

    /**
     * The format to use to replace links within the message.
     */
    [JsonPropertyName("messageFormat")]
    public string? MessageFormat { get; }

    /**
     * Links to be formatted into the message.
     */
    [JsonPropertyName("messageLinks")]
    public IEnumerable<Link>? MessageLinks { get; }

    /**
     * Method that is called when the dialog is dismissed.
     */
    [JsonPropertyName("onDismiss")]
    public Action? OnDismiss { get; }

    /**
     * The priority of the dialog determines if it will be shown over others.
     */
    [JsonPropertyName("priority")]
    public int? Priority { get; }

    /**
     * The id used to check if the dialog has been dismissed.
     */
    [JsonPropertyName("settingId")]
    public string? SettingId { get; }

    /**
     * The title of the dialog.
     */
    [JsonPropertyName("title")]
    public string? Title { get; }

    public Dialog(
        IEnumerable<object> buttonProps,
        object? closeButtonProps,
        object? contentProperties,
        string? contributionId,
        object? contributionConfiguration,
        string? message,
        string? messageFormat,
        IEnumerable<Link>? messageLinks,
        Action? onDismiss,
        int? priority,
        string? settingId,
        string? title
    )
    {
        ButtonProps = buttonProps;
        CloseButtonProps = closeButtonProps;
        ContentProperties = contentProperties;
        ContributionId = contributionId;
        ContributionConfiguration = contributionConfiguration;
        Message = message;
        MessageFormat = messageFormat;
        MessageLinks = messageLinks;
        OnDismiss = onDismiss;
        Priority = priority;
        SettingId = settingId;
        Title = title;
    }
}