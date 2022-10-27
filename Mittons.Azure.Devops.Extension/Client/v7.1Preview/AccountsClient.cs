using Mittons.Azure.Devops.Extension.Api;
using Mittons.Azure.Devops.Extension.Api.Attributes;

namespace Mittons.Azure.Devops.Extension.Client;

[GenerateClient(ResourceAreaId.Accounts, VsoScope.Profile)]
public interface IAccountsClient
{
    [ClientRequest("7.1-preview.1", "GET", "_apis/accounts?ownerId={ownerId}&memberId={memberId}&properties={properties}")]
    Task<GitAnnotatedTag> CreateAnnotatedTagAsync([ClientRequestQueryParameter] Guid ownerId, [ClientRequestQueryParameter] Guid memberId, [ClientRequestQueryParameter] string properties);
}
