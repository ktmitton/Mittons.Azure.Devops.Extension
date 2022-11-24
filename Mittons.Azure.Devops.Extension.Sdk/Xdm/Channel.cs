using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Mittons.Azure.Devops.Extension.Sdk.Xdm;

internal static class IServiceCollectionChannelExtensions
{
    public static IServiceCollection AddXdmChannel(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<IChannel, Channel>();
}

internal class Channel : IChannel, IAsyncDisposable
{
    private const int ChannelId = 1;

    private readonly IJSRuntime _jsRuntime;

    private IJSObjectReference? _jsModule;

    private static ConcurrentDictionary<int, TaskCompletionSource<string>> _messageRegistrations = new();

    public static ConcurrentDictionary<string, Func<object?[]?, object?>> _functionRegistrations = new();

    private record BaseResponseMessage
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("methodName")]
        public string? MethodName { get; set; }
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
        // NOTE: We call window.importWrapper instead of just calling import directly because the dotnet
        //       javascript for some reason has a "cache" of import which manually converts ./<file>.js
        //       from the relative path to an explicit path. Since the base ref is never set, this
        //       does not behave as expected.
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
            if (x is MulticastDelegate multicastDelegate)
            {
                var proxyFunction = new ProxyFunction(multicastDelegate, ChannelId);

                _functionRegistrations[$"proxy{proxyFunction.Id}"] = proxyFunction.Callback;

                return proxyFunction;
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
        var message = JsonSerializer.Deserialize<BaseResponseMessage>(data);

        if (message?.MethodName is not null)
        {
            _functionRegistrations[message.MethodName](default);
        }
        else if (message?.Id is not null)
        {
            if (_messageRegistrations.TryRemove(message.Id, out var taskCompletionSource))
            {
                System.Console.WriteLine($"Message [{message.Id}] processing with data [{data}]");

                taskCompletionSource.SetResult(data);
            }
            else
            {
                System.Console.WriteLine($"Message [{message.Id}] didn't match any registration [{data}]");
            }
        }
    }

    public Task InvokeRemoteMethodVoidAsync(string methodName, string instanceId, CancellationToken cancellationToken, params object?[] arguments)
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

    public Task<T> InvokeRemoteProxyMethodAsync<T>(int functionId, CancellationToken cancellationToken, params object?[] arguments)
    {
        return InvokeRemoteMethodAsync<T>($"proxy{functionId}", "__proxyFunctions", cancellationToken, arguments);
    }

    public Task<T> GetServiceDefinitionAsync<T>(string contributionId, CancellationToken cancellationToken)
        => InvokeRemoteMethodAsync<T>("getService", InstanceId.ServiceManager, cancellationToken, contributionId);

    public Task InvokeRemoteProxyMethodVoidAsync(int functionId, CancellationToken cancellationToken, params object?[] arguments)
    {
        return InvokeRemoteMethodVoidAsync($"proxy{functionId}", "__proxyFunctions", cancellationToken, arguments);
    }
}
