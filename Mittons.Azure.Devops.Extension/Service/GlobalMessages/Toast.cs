using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Service.GlobalMessages;

public class Toast
{
    [JsonPropertyName("callToAction")]
    public string? CallToAction { get; }

    [JsonPropertyName("duration")]
    public int DurationInMilliseconds { get; }

    [JsonPropertyName("forceOverrideExisting")]
    public bool? ForceOverrideExisting { get; }

    [JsonPropertyName("message")]
    public string Message { get; }

    [JsonPropertyName("onCallToActionClick")]
    public Action? OnCallToActionClick { get; }

    public Toast(
        string? callToAction,
        TimeSpan duration,
        bool? forceOverrideExisting,
        string message,
        Action? onCallToActionClick
    )
    {
        CallToAction = callToAction;
        DurationInMilliseconds = Convert.ToInt32(duration.TotalMilliseconds);
        ForceOverrideExisting = forceOverrideExisting;
        Message = message;
        OnCallToActionClick = onCallToActionClick;
    }
}