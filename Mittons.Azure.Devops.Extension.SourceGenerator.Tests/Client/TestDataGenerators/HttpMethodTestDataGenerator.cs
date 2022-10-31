using System.Collections;
using System.Net.Http.Json;
using System.Net.Mime;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client.TestDataGenerators;

public class HttpMethodTestDataGenerator : IEnumerable<object[]>
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
                (ITestGitClient client) => client.PostEmptyBody(),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                default
            )
        }
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}