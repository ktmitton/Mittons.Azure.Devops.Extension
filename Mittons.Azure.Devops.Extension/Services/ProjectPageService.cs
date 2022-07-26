using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Models.Project;
using Mittons.Azure.Devops.Extension.Sdk;
using Mittons.Azure.Devops.Extension.Xdm;

namespace Mittons.Azure.Devops.Extension.Service;

internal static class IServiceCollectionProjectPageServiceExtensions
{
    public static IServiceCollection AddProjectPageService(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<IProjectPageService, ProjectPageService>();
}

/// <summary>
/// Exposes project-related information from the current page
/// </summary>
public interface IProjectPageService
{
    Task<Info?> GetProjectAsync();
}

internal class ProjectPageService : IProjectPageService
{
    private const string ContributionId = "ms.vss-tfs-web.tfs-page-data-service";

    private ProjectPageServiceDefinition? _definition;

    private readonly IChannel _channel;

    private readonly Task _ready;

    public ProjectPageService(ISdk sdk, IChannel channel)
    {
        _channel = channel;
        _ready = InitializeAsync(sdk.Ready);
    }

    private async Task InitializeAsync(Task sdkReady)
    {
        await sdkReady;

        _definition = await _channel.GetServiceDefinitionAsync<ProjectPageServiceDefinition>(ContributionId);

        if (_definition is null)
        {
            throw new NullReferenceException($"Unabled to get service definition for {ContributionId}");
        }

        if (_definition.GetProjectProxyFunctionDefinition is null)
        {
            throw new NullReferenceException($"Service definition did not include required proxy function definition for {ProjectPageServiceDefinition.GetProjectPropertyName}");
        }
    }

    public async Task<Info?> GetProjectAsync()
    {
        await _ready;

        return await _channel.InvokeRemoteProxyMethodAsync<Info>(_definition?.GetProjectProxyFunctionDefinition);
    }

    private record ProjectPageServiceDefinition
    {
        public const string GetProjectPropertyName = "getProject";

        [JsonPropertyName(GetProjectPropertyName)]
        public ProxyFunctionDefinition? GetProjectProxyFunctionDefinition { get; init; }
    }
}