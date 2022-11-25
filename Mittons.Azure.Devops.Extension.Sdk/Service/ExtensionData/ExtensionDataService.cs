using Mittons.Azure.Devops.Extension.Sdk.Attributes;

namespace Mittons.Azure.Devops.Extension.Sdk.Service.ExtensionData;

public interface IExtensionDataManager { }

[GenerateService("ms.vss-features.extension-data-service")]
public interface IExtensionDataService
{
    [RemoteProxyFunction("getExtensionDataManager")]
    Task<IExtensionDataManager> GetExtensionDataManagerAsync(string extension, string accessToken, CancellationToken cancellationToken = default);
}
