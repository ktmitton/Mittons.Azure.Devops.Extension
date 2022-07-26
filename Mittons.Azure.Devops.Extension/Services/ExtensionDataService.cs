using Microsoft.Extensions.DependencyInjection;

namespace Mittons.Azure.Devops.Extension.Service;

internal static class IServiceCollectionExtensionDataServiceExtensions
{
    public static IServiceCollection AddExtensionDataService(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<IExtensionDataService, ExtensionDataService>();
}

/// <summary>
/// Service for interacting with the extension data service
/// </summary>
public interface IExtensionDataService
{
}

internal class ExtensionDataService : IExtensionDataService
{
    private const string ContributionId = "ms.vss-features.extension-data-service";
}
