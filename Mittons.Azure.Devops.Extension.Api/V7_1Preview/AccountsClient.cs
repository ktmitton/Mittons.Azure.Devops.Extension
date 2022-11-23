using Mittons.Azure.Devops.Extension.Api.Attributes;
using Mittons.Azure.Devops.Extension.Sdk;

namespace Mittons.Azure.Devops.Extension.Api.V7_1Preview;

[GenerateClient(ResourceAreaId.Accounts, VsoScope.Profile)]
public interface IAccountsClient
{
    [ClientRequest("7.1-preview.1", "GET", "_apis/accounts?ownerId={ownerId}&memberId={memberId}&properties={properties}")]
    Task<string> GetAccounts([ClientRequestQueryParameter] Guid? ownerId, [ClientRequestQueryParameter] Guid? memberId, [ClientRequestQueryParameter] string? properties);
}
