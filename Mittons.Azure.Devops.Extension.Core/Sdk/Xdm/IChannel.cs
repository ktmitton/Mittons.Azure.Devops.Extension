using System.Threading;
using System.Threading.Tasks;

namespace Mittons.Azure.Devops.Extension.Sdk.Xdm
{
    public interface IChannel
    {
        Task InitializeAsync(CancellationToken cancellationToken);

        Task InvokeRemoteMethodVoidAsync(string methodName, string instanceId, CancellationToken cancellationToken, params object[] arguments);

        Task<T> InvokeRemoteMethodAsync<T>(string methodName, string instanceId, CancellationToken cancellationToken, params object[] arguments);

        Task<T> InvokeRemoteProxyMethodAsync<T>(int functionId, CancellationToken cancellationToken, params object?[] arguments);

        Task InvokeRemoteProxyMethodVoidAsync(int functionId, CancellationToken cancellationToken, params object?[] arguments);

        Task<T> GetServiceDefinitionAsync<T>(string contributionId, CancellationToken cancellationToken);
    }
}