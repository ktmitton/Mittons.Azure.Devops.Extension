namespace Mittons.Azure.Devops.Extension.Service;

[GenerateService("ms.vss-features.location-service")]
public interface ILocationService
{
    [ProxyFunction("getResourceAreaLocation")]
    Task<string> GetResourceAreaLocationAsync(string resourceAreaId);

    [ProxyFunction("getServiceLocation")]
    Task<string> GetServiceLocationAsync(string? serviceInstanceType, HostType? hostType);

    [ProxyFunction("routeUrl")]
    Task<string> CreateRouteUrlAsync(string routeId, Dictionary<string, string> routeValues, string hostPath);
}
