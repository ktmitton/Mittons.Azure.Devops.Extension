using System.Text.Json.Serialization;
using Mittons.Azure.Devops.Extension.Sdk.Xdm;

namespace Mittons.Azure.Devops.Extension.Sdk.Service.GlobalMessages;

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
    public ProxyFunction OnCallToActionClick { get; }

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
        if (onCallToActionClick is null)
        {
            OnCallToActionClick = new ProxyFunction(() => {}, 1);
        }
        else
        {
            OnCallToActionClick = new ProxyFunction(onCallToActionClick, 1);
        }
        Channel._functionRegistrations[$"proxy{OnCallToActionClick.Id}"] = OnCallToActionClick.Callback;
    }
}
