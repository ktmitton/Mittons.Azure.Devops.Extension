using System.Collections;
using System.Net.Http.Json;
using System.Net.Mime;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client.TestDataGenerators;

public class RouteParameterTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
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