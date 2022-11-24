using Mittons.Azure.Devops.Extension.Sdk.Attributes;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Service;

[GenerateService("mitt.test-service-one")]
public interface ITestServiceOne
{
    [RemoteProxyFunction("simpleFunction")]
    Task SimpleFunctionAsync(CancellationToken cancellationToken);

    [RemoteProxyFunction("renamedSimpleFunction")]
    Task SimpleFunctionRenamedAsync(CancellationToken cancellationToken);
}
