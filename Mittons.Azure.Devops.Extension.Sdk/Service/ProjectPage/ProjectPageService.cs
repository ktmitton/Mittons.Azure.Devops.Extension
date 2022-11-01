using Mittons.Azure.Devops.Extension.Sdk.Service.Attributes;

namespace Mittons.Azure.Devops.Extension.Service.ProjectPage;

/// <summary>
/// Exposes project-related information from the current page
/// </summary>
[GenerateService("ms.vss-tfs-web.tfs-page-data-service")]
public interface IProjectPageService
{
    [ProxyFunction("getProject")]
    Task<ProjectInfo?> GetProjectAsync();
}
