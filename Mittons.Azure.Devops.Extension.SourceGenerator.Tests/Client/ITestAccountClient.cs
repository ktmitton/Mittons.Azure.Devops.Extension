using Mittons.Azure.Devops.Extension.Api.Attributes;
using Mittons.Azure.Devops.Extension.Sdk;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

[GenerateClient(ResourceAreaId.Accounts)]
public interface ITestAccountsClient
{
}
