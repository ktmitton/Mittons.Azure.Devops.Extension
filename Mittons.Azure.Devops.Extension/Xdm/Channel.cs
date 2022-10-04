using System.Collections.Concurrent;
using System.Reflection;
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
    Task InitializeAsync(CancellationToken cancellationToken = default);

    Task InvokeRemoteMethodAsync(string methodName, string instanceId, CancellationToken cancellationToken, params object[] arguments);

    Task<T> InvokeRemoteMethodAsync<T>(string methodName, string instanceId, CancellationToken cancellationToken, params object[] arguments);

    Task<T> InvokeRemoteProxyMethodAsync<T>(ProxyFunctionDefinition? proxyFunctionDefinition, CancellationToken cancellationToken, params object?[] arguments);

    Task InvokeRemoteProxyMethodAsync(ProxyFunctionDefinition? proxyFunctionDefinition, CancellationToken cancellationToken, params object?[] arguments);

    Task<T> GetServiceDefinitionAsync<T>(string contributionId, CancellationToken cancellationToken);
}

internal class Channel : IChannel, IAsyncDisposable
{
    private const int ChannelId = 1;

    private readonly IJSRuntime _jsRuntime;

    private IJSObjectReference? _jsModule;

    private static ConcurrentDictionary<int, TaskCompletionSource<string>> _messageRegistrations = new();

    private static ConcurrentDictionary<int, Func<object?[]?, object?>> _functionRegistrations = new();

    private static int _nextFunctionId = 1;

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

    public Channel(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("window.importWrapper", "./xdm.js");

        await _jsModule.InvokeVoidAsync("addRpcMessageListener", DotNetObjectReference.Create(this));
    }

    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("removeRpcMessageListener");
            await _jsModule.DisposeAsync();
        }
    }

    private async Task<string> SendRpcMessage(string methodName, string instanceId, CancellationToken cancellationToken, params object?[] arguments)
    {
        if (_jsModule is null)
        {
            throw new NullReferenceException("xdm.js module has not been loaded yet, make sure Channel.InitializeAsync() is called.");
        }

        var sanitizedArguments = arguments.Select(x =>
        {
            if (x is MulticastDelegate)
            {
                var type = x.GetType();
                var methodInfo = type.GetProperty("Method")?.GetValue(x) as MethodInfo;

                if (methodInfo is null)
                {
                    throw new NullReferenceException($"Unable to get method info for type [{type.Name}]");
                }

                var context = type.GetProperty("Target")?.GetValue(x);
                var functionId = _nextFunctionId++;

                _functionRegistrations[functionId] = (object?[]? args) => methodInfo.Invoke(context, args);

                return new
                {
                    __proxyFunctionId = functionId,
                    _channelId = ChannelId
                };
            }
            else
            {
                return x;
            }
        }).ToArray();

        var message = new Message(methodName, instanceId, sanitizedArguments);

        var taskCompletionSource = new TaskCompletionSource<string>();

        _messageRegistrations[message.Id] = taskCompletionSource;

        await _jsModule.InvokeVoidAsync("sendRpcMessage", JsonSerializer.Serialize(message));

        return await taskCompletionSource.Task;
    }

    [JSInvokable]
    public void ReceiveRpcMessage(string data)
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

    public Task InvokeRemoteMethodAsync(string methodName, string instanceId, CancellationToken cancellationToken, params object?[] arguments)
        => SendRpcMessage(methodName, instanceId, cancellationToken, arguments);

    public async Task<T> InvokeRemoteMethodAsync<T>(string methodName, string instanceId, CancellationToken cancellationToken, params object?[] arguments)
    {
        var rawData = await SendRpcMessage(methodName, instanceId, cancellationToken, arguments);

        var responseMessage = JsonSerializer.Deserialize<ResponseMessage<T>>(rawData);

        System.Console.WriteLine(responseMessage);

        if (responseMessage is null || responseMessage.Result is null)
        {
            throw new NullReferenceException($"Response data [{rawData}] could not be deserialized into type [{typeof(ResponseMessage<T>)}]");
        }

        return responseMessage.Result;
    }

    public Task<T> InvokeRemoteProxyMethodAsync<T>(ProxyFunctionDefinition? proxyFunctionDefinition, CancellationToken cancellationToken, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(proxyFunctionDefinition, nameof(proxyFunctionDefinition));

        return InvokeRemoteMethodAsync<T>($"proxy{proxyFunctionDefinition.FunctionId}", "__proxyFunctions", cancellationToken, arguments);
    }

    public Task<T> GetServiceDefinitionAsync<T>(string contributionId, CancellationToken cancellationToken)
        => InvokeRemoteMethodAsync<T>("getService", InstanceId.ServiceManager, cancellationToken, contributionId);

    public Task InvokeRemoteProxyMethodAsync(ProxyFunctionDefinition? proxyFunctionDefinition, CancellationToken cancellationToken, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(proxyFunctionDefinition, nameof(proxyFunctionDefinition));

        return InvokeRemoteMethodAsync($"proxy{proxyFunctionDefinition.FunctionId}", "__proxyFunctions", cancellationToken, arguments);
    }
}