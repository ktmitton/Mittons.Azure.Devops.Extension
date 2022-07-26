using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Xdm;

public record ProxyFunctionDefinition
{
    [JsonPropertyName("__proxyFunctionId")]
    public int FunctionId { get; init; }

    [JsonPropertyName("_channelId")]
    public int ChannelId { get; init; }
}
