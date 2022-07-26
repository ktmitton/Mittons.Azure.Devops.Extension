using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Xdm;

namespace Mittons.Azure.Devops.Extension.Service;

internal static class IServiceCollectionLocationServiceExtensions
{
    public static IServiceCollection AddLocationService(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<ILocationService, LocationService>();
}

public interface ILocationService
{
    Task<string> GetResourceAreaLocationAsync(string resourceAreaId);

    Task<string> GetServiceLocationAsync();

    Task<string> GetServiceLocationAsync(string? serviceInstanceType, HostType? hostType);

    Task<string> CreateRouteUrlAsync(string routeId, Dictionary<string, string> routeValues, string hostPath);
}

internal class LocationService : ILocationService
{
    private const string ContributionId = "ms.vss-features.location-service";

    private LocationServiceDefinition? _definition;

    private readonly IChannel _channel;

    private readonly Task _ready;

    public LocationService(IChannel channel)
    {
        _channel = channel;
        _ready = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        _definition = await _channel.GetServiceDefinitionAsync<LocationServiceDefinition>(ContributionId);

        if (_definition is null)
        {
            throw new NullReferenceException($"Unabled to get service definition for {ContributionId}");
        }

        if (_definition.GetServiceLocationProxyFunctionDefinition is null)
        {
            throw new NullReferenceException($"Service definition did not include required proxy function definition for {LocationServiceDefinition.GetResourceAreaLocationPropertyName}");
        }

        if (_definition.GetServiceLocationProxyFunctionDefinition is null)
        {
            throw new NullReferenceException($"Service definition did not include required proxy function definition for {LocationServiceDefinition.GetServiceLocationPropertyName}");
        }

        if (_definition.CreateRouteUrlProxyFunctionDefinition is null)
        {
            throw new NullReferenceException($"Service definition did not include required proxy function definition for {LocationServiceDefinition.CreateRouteUrlPropertyName}");
        }
    }

    public async Task<string> GetResourceAreaLocationAsync(string resourceAreaId)
    {
        await _ready;

        return await _channel.InvokeRemoteProxyMethodAsync<string>(_definition?.GetResourceAreaLocationProxyFunctionDefinition, resourceAreaId);
    }

    public Task<string> GetServiceLocationAsync()
        => GetServiceLocationAsync(default, default);

    public async Task<string> GetServiceLocationAsync(string? serviceInstanceType, HostType? hostType)
    {
        await _ready;

        return await _channel.InvokeRemoteProxyMethodAsync<string>(_definition?.GetServiceLocationProxyFunctionDefinition, serviceInstanceType, hostType);
    }

    public async Task<string> CreateRouteUrlAsync(string routeId, Dictionary<string, string> routeValues, string hostPath)
    {
        await _ready;

        return await _channel.InvokeRemoteProxyMethodAsync<string>(_definition?.CreateRouteUrlProxyFunctionDefinition, routeId, routeValues, hostPath);
    }

    private record LocationServiceDefinition
    {
        public const string GetResourceAreaLocationPropertyName = "getResourceAreaLocation";

        public const string GetServiceLocationPropertyName = "getResourceAreaLocation";

        public const string CreateRouteUrlPropertyName = "getResourceAreaLocation";

        [JsonPropertyName(GetResourceAreaLocationPropertyName)]
        public ProxyFunctionDefinition? GetResourceAreaLocationProxyFunctionDefinition { get; init; }

        [JsonPropertyName(GetServiceLocationPropertyName)]
        public ProxyFunctionDefinition? GetServiceLocationProxyFunctionDefinition { get; init; }

        [JsonPropertyName(CreateRouteUrlPropertyName)]
        public ProxyFunctionDefinition? CreateRouteUrlProxyFunctionDefinition { get; init; }
    }
}