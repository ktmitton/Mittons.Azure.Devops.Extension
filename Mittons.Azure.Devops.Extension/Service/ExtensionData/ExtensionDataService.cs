namespace Mittons.Azure.Devops.Extension.Service.ExtensionData;

public interface IExtensionDataManager { }

[GenerateService("ms.vss-features.extension-data-service")]
public interface IExtensionDataService
{
    [ProxyFunction("getExtensionDataManager")]
    Task<IExtensionDataManager> GetExtensionDataManagerAsync(string extension, string accessToken);
}
