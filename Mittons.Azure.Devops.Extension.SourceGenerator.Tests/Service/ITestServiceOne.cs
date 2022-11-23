using Mittons.Azure.Devops.Extension.Sdk;
using Mittons.Azure.Devops.Extension.Sdk.Attributes;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Service;

[GenerateService(ResourceAreaId.Accounts)]
public interface ITestServiceOne
{
}
