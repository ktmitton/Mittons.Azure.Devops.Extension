using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Mittons.Azure.Devops.Extension.Xdm;

internal static class IServiceCollectionChannelExtensions
{
    public static IServiceCollection AddXdmChannel(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<IChannel, Channel>();
}

public interface IChannel
{
    Task<T> InvokeRemoteMethodAsync<T>(string methodName, string instanceId, params object[] arguments);

    Task<T> InvokeRemoteProxyMethodAsync<T>(ProxyFunctionDefinition? proxyFunctionDefinition, params object?[] arguments);

    Task InvokeRemoteProxyMethodAsync(ProxyFunctionDefinition? proxyFunctionDefinition, params object?[] arguments);

    Task<T> GetServiceDefinitionAsync<T>(string contributionId);
}

internal class Channel : IChannel
{
    private readonly IJSRuntime _jsRuntime;

    private static ConcurrentDictionary<int, TaskCompletionSource<string>> _messageRegistrations = new ConcurrentDictionary<int, TaskCompletionSource<string>>();

    private record BaseResponseMessage
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    private record ResponseMessage<T> : BaseResponseMessage
    {
        [JsonPropertyName("result")]
        public T? Result { get; set; }
    }

    public static void OnMessageEvent(string data)
    {
        var messageId = JsonSerializer.Deserialize<BaseResponseMessage>(data)?.Id ?? default;

        if (_messageRegistrations.TryRemove(messageId, out var taskCompletionSource))
        {
            System.Console.WriteLine($"Message [{messageId}] processing with data [{data}]");

            taskCompletionSource.SetResult(data);
        }
        else
        {
            System.Console.WriteLine($"Message [{messageId}] didn't match any registration [{data}]");
        }
    }

    public Channel(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<T> InvokeRemoteMethodAsync<T>(string methodName, string instanceId, params object?[] arguments)
    {
        var message = new Message(methodName, instanceId, arguments);

        var taskCompletionSource = new TaskCompletionSource<string>();

        _messageRegistrations[message.Id] = taskCompletionSource;

        await _jsRuntime.InvokeAsync<object>("sendRpcMessage", JsonSerializer.Serialize(message));

        var rawData = await taskCompletionSource.Task;

        var responseMessage = JsonSerializer.Deserialize<ResponseMessage<T>>(rawData);

        System.Console.WriteLine(responseMessage);

        if (responseMessage is null || responseMessage.Result is null)
        {
            throw new NullReferenceException($"Response data [{rawData}] could not be deserialized into type [{typeof(ResponseMessage<T>)}]");
        }

        return responseMessage.Result;
    }

    private async Task InvokeRemoteMethodAsync(string methodName, string instanceId, params object?[] arguments)
    {
        var message = new Message(methodName, instanceId, arguments);

        await _jsRuntime.InvokeAsync<object>("sendRpcMessage", JsonSerializer.Serialize(message));
    }

    public Task<T> InvokeRemoteProxyMethodAsync<T>(ProxyFunctionDefinition? proxyFunctionDefinition, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(proxyFunctionDefinition, nameof(proxyFunctionDefinition));

        return InvokeRemoteMethodAsync<T>($"proxy{proxyFunctionDefinition.FunctionId}", "__proxyFunctions", arguments);
    }

    public Task<T> GetServiceDefinitionAsync<T>(string contributionId)
        => InvokeRemoteMethodAsync<T>("getService", InstanceId.ServiceManager, contributionId);

    public Task InvokeRemoteProxyMethodAsync(ProxyFunctionDefinition? proxyFunctionDefinition, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(proxyFunctionDefinition, nameof(proxyFunctionDefinition));

        return InvokeRemoteMethodAsync($"proxy{proxyFunctionDefinition.FunctionId}", "__proxyFunctions", arguments);
    }
}