using Mittons.Azure.Devops.Extension.Sdk.Attributes;

namespace Mittons.Azure.Devops.Extension.Service.ProjectPage;

/// <summary>
/// Exposes project-related information from the current page
/// </summary>
[GenerateService("ms.vss-tfs-web.tfs-page-data-service")]
public interface IProjectPageService
{
    [RemoteProxyFunction("getProject")]
    Task<ProjectInfo?> GetProjectAsync();
}
