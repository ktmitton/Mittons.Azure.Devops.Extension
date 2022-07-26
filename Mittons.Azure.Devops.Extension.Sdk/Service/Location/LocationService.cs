using Mittons.Azure.Devops.Extension.Sdk.Attributes;

namespace Mittons.Azure.Devops.Extension.Sdk.Service.Location;

[GenerateService("ms.vss-features.location-service")]
public interface ILocationService
{
    [RemoteProxyFunction("getResourceAreaLocation")]
    Task<string> GetResourceAreaLocationAsync(string resourceAreaId, CancellationToken cancellationToken = default);

    [RemoteProxyFunction("getServiceLocation")]
    Task<string> GetServiceLocationAsync(string? serviceInstanceType, HostType? hostType, CancellationToken cancellationToken = default);

    [RemoteProxyFunction("routeUrl")]
    Task<string> CreateRouteUrlAsync(string routeId, Dictionary<string, string> routeValues, string hostPath, CancellationToken cancellationToken = default);
}
