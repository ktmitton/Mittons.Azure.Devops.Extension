using System.Collections;
using System.Net.Http.Json;
using System.Net.Mime;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

public class MediaTypeParameterTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithApiVersion1(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithApiVersion2(),
                HttpMethod.Get,
                "7.3",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithExplicitJsonMediaType(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithExplicitPlainTextMediaType(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Text.Plain,
                new StringContent("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithPath1(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithPath2(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/path",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
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
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithRouteParameters1(1, "test"),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get/1/test",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithRouteParameters1(274, "random"),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get/274/random",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithRouteParameters2(new Guid("6b22e4b8-e4c5-40ce-92ee-6e07aab08675"), 2.0m),
                HttpMethod.Get,
                "5.2-preview.1",
                "/6b22e4b8-e4c5-40ce-92ee-6e07aab08675/get/2.0",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create("Test"),
                "Test",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.GetWithRouteParameters2(new Guid("15bbb737-a3c4-4065-8725-9121ad809913"), 152.37m),
                HttpMethod.Get,
                "5.2-preview.1",
                "/15bbb737-a3c4-4065-8725-9121ad809913/get/152.37",
                string.Empty,
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