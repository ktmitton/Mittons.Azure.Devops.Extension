using Microsoft.Extensions.DependencyInjection;

namespace Mittons.Azure.Devops.Extension.Service;

internal static class IServiceCollectionHostPageLayoutServiceExtensions
{
    public static IServiceCollection AddHostPageLayoutService(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<IHostPageLayoutService, HostPageLayoutService>();
}

/// <summary>
/// Service for interacting with the layout of the page: managing full-screen mode,
/// opening dialogs and panels
/// </summary>
public interface IHostPageLayoutService
{
}

internal class HostPageLayoutService : IHostPageLayoutService
{
    private const string ContributionId = "ms.vss-features.host-page-layout-service";
}
