using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Service.Location;

namespace Mittons.Azure.Devops.Extension.Net.Http;

public interface IResourceAreaUriResolver
{
    Task<Uri> ResolveAsync(string resourceAreaId);

    Uri Resolve(string resourceAreaId);
}

public static class ResourceAreaUriResolverExtensions
{
    public static IServiceCollection AddResourceAreaUriResolver(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<IResourceAreaUriResolver, ResourceAreaUrlResolver>();
}

internal class ResourceAreaUrlResolver : IResourceAreaUriResolver
{
    private readonly ILocationService _locationService;

    private readonly ConcurrentDictionary<string, Lazy<Task<Uri>>> _cachedAreas = new();

    public ResourceAreaUrlResolver(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public Uri Resolve(string resourceAreaId)
        => ResolveAsync(resourceAreaId).GetAwaiter().GetResult();

    public Task<Uri> ResolveAsync(string resourceAreaId)
    {
        _cachedAreas.TryAdd(resourceAreaId, new Lazy<Task<Uri>>(() => GetResourceAreaUriAsync(resourceAreaId)));

        return _cachedAreas[resourceAreaId].Value;
    }

    private async Task<Uri> GetResourceAreaUriAsync(string resourceAreaId)
        => new Uri(await _locationService.GetResourceAreaLocationAsync(resourceAreaId));
}
