using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Client;
using Mittons.Azure.Devops.Extension.Api.Net.Http;
using Mittons.Azure.Devops.Extension.Service.Location;

namespace Mittons.Azure.Devops.Extension.Net.Http;

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

    public Task PrimeKnownResourceAreasAsync(CancellationToken cancellationToken)
        => Task.WhenAll(
            ResolveAsync(default, cancellationToken),
            ResolveAsync(ResourceAreaId.Git, cancellationToken),
            ResolveAsync(ResourceAreaId.Git, cancellationToken),
            ResolveAsync(ResourceAreaId.Accounts, cancellationToken),
            ResolveAsync(ResourceAreaId.Boards, cancellationToken),
            ResolveAsync(ResourceAreaId.Builds, cancellationToken),
            ResolveAsync(ResourceAreaId.Core, cancellationToken),
            ResolveAsync(ResourceAreaId.Dashboard, cancellationToken),
            ResolveAsync(ResourceAreaId.ExtensionManagement, cancellationToken),
            ResolveAsync(ResourceAreaId.Git, cancellationToken),
            ResolveAsync(ResourceAreaId.Graph, cancellationToken),
            ResolveAsync(ResourceAreaId.Policy, cancellationToken),
            ResolveAsync(ResourceAreaId.ProjectAnalysis, cancellationToken),
            ResolveAsync(ResourceAreaId.Release, cancellationToken),
            ResolveAsync(ResourceAreaId.ServiceEndpoint, cancellationToken),
            ResolveAsync(ResourceAreaId.TaskAgent, cancellationToken),
            ResolveAsync(ResourceAreaId.Test, cancellationToken),
            ResolveAsync(ResourceAreaId.Tfvc, cancellationToken),
            ResolveAsync(ResourceAreaId.Wiki, cancellationToken),
            ResolveAsync(ResourceAreaId.Work, cancellationToken),
            ResolveAsync(ResourceAreaId.WorkItemTracking, cancellationToken)
        );

    public Uri Resolve(string? resourceAreaId)
        => ResolveAsync(resourceAreaId, default).GetAwaiter().GetResult();

    public Task<Uri> ResolveAsync(string? resourceAreaId, CancellationToken cancellationToken)
    {
        _cachedAreas.TryAdd(
            resourceAreaId ?? string.Empty,
            new Lazy<Task<Uri>>(() => string.IsNullOrWhiteSpace(resourceAreaId) ? GetServiceLocationAsync() : GetResourceAreaUriAsync(resourceAreaId))
        );

        return _cachedAreas[resourceAreaId ?? string.Empty].Value;
    }

    private async Task<Uri> GetResourceAreaUriAsync(string resourceAreaId)
        => new Uri(await _locationService.GetResourceAreaLocationAsync(resourceAreaId));

    private async Task<Uri> GetServiceLocationAsync()
        => new Uri(await _locationService.GetServiceLocationAsync(default, default));
}
