using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Models.Git;
using Mittons.Azure.Devops.Extension.Sdk;
using Mittons.Azure.Devops.Extension.Service;

namespace Mittons.Azure.Devops.Extension.Client;

internal static class IServiceCollectionGitClientExtensions
{
    public static IServiceCollection AddGitClient(this IServiceCollection @serviceCollection)
        => @serviceCollection.AddSingleton<IGitClient, GitClient>();
}

public interface IGitClient
{
    Task<GitRepository> GetRepositoriesAsync(bool includeLinks, bool includeAllUrls, bool includeHidden);
}

internal class GitClient : RestClient, IGitClient
{
    private const string DefaultApiVersion = "5.2-preview.1";

    private readonly IProjectPageService _projectPageService;

    public GitClient(ISdk sdk, ILocationService locationService, IProjectPageService projectPageService) : base(sdk, locationService)
    {
        _projectPageService = projectPageService;
    }

    public async Task<GitRepository> GetRepositoriesAsync(bool includeLinks, bool includeAllUrls, bool includeHidden)
    {
        var projectId = (await _projectPageService.GetProjectAsync())?.Id;

        return await base.SendRequestAsync<GitRepository>(
            apiVersion: DefaultApiVersion,
            route: $"{projectId}/_apis/git/Repositories/",
            queryParameters: new Dictionary<string, string>
            {
                { "includeLinks", includeLinks.ToString().ToLower() },
                { "includeAllUrls", includeAllUrls.ToString().ToLower() },
                { "includeHidden", includeHidden.ToString().ToLower() },
            }
        );
    }
}