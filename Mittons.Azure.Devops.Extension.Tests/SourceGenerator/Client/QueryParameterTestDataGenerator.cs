using System.Collections;
using System.Net.Http.Json;
using System.Net.Mime;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

public class QueryParameterTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithQueryParameters1("Test"),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                "?someParameter=Test",
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithQueryParameters1("other"),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                "?someParameter=other",
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithQueryParameters2(true, "test", 1.0m),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                "?otherParameter=test&testParameter1=1.0&testParameter2=true",
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithQueryParameters2(false, "other", 152.73m),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                "?otherParameter=other&testParameter1=152.73&testParameter2=false",
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithQueryParameters2(default, "test", 1.0m),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                "?otherParameter=test&testParameter1=1.0",
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithQueryParameters2(true, default, 1.0m),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                "?testParameter1=1.0&testParameter2=true",
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithQueryParameters2(true, "test", default),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                "?otherParameter=test&testParameter2=true",
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        }
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}