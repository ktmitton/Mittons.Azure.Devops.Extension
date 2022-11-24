using Mittons.Azure.Devops.Extension.Sdk.Attributes;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Service;

[GenerateService("two")]
public interface ITestServiceTwo
{
    [RemoteProxyFunction("simpleFunction")]
    Task SimpleFunctionAsync(CancellationToken cancellationToken);

    [RemoteProxyFunction("renamedSimpleFunction")]
    Task SimpleFunctionRenamedAsync(CancellationToken cancellationToken);
}
