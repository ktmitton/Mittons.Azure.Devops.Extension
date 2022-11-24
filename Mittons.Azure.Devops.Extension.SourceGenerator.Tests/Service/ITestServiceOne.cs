using Mittons.Azure.Devops.Extension.Sdk.Attributes;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Service;

[GenerateService("mitt.test-service-one")]
public interface ITestServiceOne
{
    [RemoteProxyFunction("simpleFunction")]
    Task SimpleFunctionAsync(CancellationToken cancellationToken = default);

    [RemoteProxyFunction("renamedSimpleFunction")]
    Task SimpleFunctionRenamedAsync(CancellationToken cancellationToken = default);

    [RemoteProxyFunction("simpleFunctionWithArguments")]
    Task SimpleFunctionWithArgumentsAsync(int a, string b, bool other, CancellationToken cancellationToken = default);

    [RemoteProxyFunction("genericFunction")]
    Task GenericFunctionAsync(CancellationToken cancellationToken = default);

    [RemoteProxyFunction("genericFunctionWithArguments")]
    Task GenericFunctionWithArgumentsAsync(string other, int a, CancellationToken cancellationToken = default);
}
