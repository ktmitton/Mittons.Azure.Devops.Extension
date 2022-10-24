namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

public record FunctionDefinition<T>(
    Func<ITestGitClient, Task<T>> TestRequestAsync,
    HttpMethod ExpectedHttpMethod,
    string ExpectedApiVersion,
    string ExpectedPath,
    string ExpectedQuery,
    string ExpectedMediaType,
    HttpContent ResponseContent,
    T? ExpectedReturnValue,
    HttpContent? ExpectedRequestContent);