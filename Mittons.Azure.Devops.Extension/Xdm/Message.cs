using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Xdm;

internal record Message
{
    private static int _nextMessageId = 1;

    [JsonPropertyName("id")]
    public int Id { get; } = _nextMessageId++;

    [JsonPropertyName("methodName")]
    public string MethodName { get; }

    [JsonPropertyName("instanceId")]
    public string InstanceId { get; }

    [JsonPropertyName("params")]
    public object?[] Arguments { get; }

    public Message(string methodName, string instanceId, object?[] arguments)
    {
        MethodName = methodName;
        InstanceId = instanceId;
        Arguments = arguments;
    }
}
