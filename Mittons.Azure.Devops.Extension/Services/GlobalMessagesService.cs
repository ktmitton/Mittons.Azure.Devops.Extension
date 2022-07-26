using Microsoft.Extensions.DependencyInjection;

namespace Mittons.Azure.Devops.Extension.Service;

internal static class IServiceCollectionGlobalMessagesServiceExtensions
{
    public static IServiceCollection AddGlobalMessagesService(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<IGlobalMessagesService, GlobalMessagesService>();
}

/// <summary>
/// Service for showing global message banners at the top of the page
/// </summary>
public interface IGlobalMessagesService
{
}

internal class GlobalMessagesService : IGlobalMessagesService
{
    private const string ContributionId = "ms.vss-tfs-web.tfs-global-messages-service";
}
