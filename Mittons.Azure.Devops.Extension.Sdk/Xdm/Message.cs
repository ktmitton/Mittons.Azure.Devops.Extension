using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Xdm;

internal record Message(
    [property: JsonPropertyName("methodName")] string MethodName,
    [property: JsonPropertyName("instanceId")] string InstanceId,
    [property: JsonPropertyName("params")] object?[] Arguments
)
{
    private static int _nextMessageId = 0;

    [JsonPropertyName("id")]
    public int Id { get; } = ++_nextMessageId;
}
