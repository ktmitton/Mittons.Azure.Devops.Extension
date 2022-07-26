using Microsoft.Extensions.DependencyInjection;

namespace Mittons.Azure.Devops.Extension.Service;

internal static class IServiceCollectionHostNavigationServiceExtensions
{
    public static IServiceCollection AddHostNavigationService(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<IHostNavigationService, HostNavigationService>();
}

/// <summary>
/// Service for interacting with the host window's navigation (URLs, new windows, etc.)
/// </summary>
public interface IHostNavigationService
{
}

internal class HostNavigationService : IHostNavigationService
{
    private const string ContributionId = "ms.vss-features.host-navigation-service";
}
