using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Xdm;

public record ProxyFunctionDefinition : IRemoteProxyFunctionDefinition
{
    [JsonPropertyName("__proxyFunctionId")]
    public int FunctionId { get; init; }

    [JsonPropertyName("_channelId")]
    public int ChannelId { get; init; }
}
