using System.Reflection;
using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Xdm;

public record ProxyFunction
{
    private static int _nextFunctionId = 1;

    [JsonPropertyName("__proxyFunctionId")]
    public int Id { get; } = _nextFunctionId++;

    [JsonPropertyName("_channelId")]
    public int ChannelId { get; }

    [JsonIgnore]
    public Func<object?[]?, object?> Callback { get; }

    public ProxyFunction(MulticastDelegate multicastDelegate, int channelId)
    {
        var type = multicastDelegate.GetType();
        var methodInfo = type.GetProperty("Method")?.GetValue(multicastDelegate) as MethodInfo;

        if (methodInfo is null)
        {
            throw new NullReferenceException($"Unable to get method info for type [{type.Name}]");
        }

        var context = type.GetProperty("Target")?.GetValue(multicastDelegate);

        Callback = (object?[]? args) => methodInfo.Invoke(context, args);
        ChannelId = channelId;
    }
}
