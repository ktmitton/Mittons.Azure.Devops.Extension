using System.Collections;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client.TestDataGenerators;

public class RequestBodyTestDataGenerator : IEnumerable<object[]>
{
    private static HttpContent CreateByteArrayContent(byte[] content, string mediaType)
    {
        var httpContent = new ByteArrayContent(content);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

        return httpContent;
    }

    private readonly List<object[]> _data = new List<object[]>
    {
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
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostByteArrayBody(new byte[0]),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                CreateByteArrayContent(new byte[0], MediaTypeNames.Application.Octet)
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostByteArrayBody(new byte[] { 0x11, 0x13 }),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                CreateByteArrayContent(new byte[] { 0x11, 0x13 }, MediaTypeNames.Application.Octet)
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostByteArrayBody(new byte[] { 0x10, 0x21, 0x22 }),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                CreateByteArrayContent(new byte[] { 0x10, 0x21, 0x22 }, MediaTypeNames.Application.Octet)
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostByteArrayOctectBody(new byte[0]),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                CreateByteArrayContent(new byte[0], MediaTypeNames.Application.Octet)
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostByteArrayOctectBody(new byte[] { 0x11, 0x13 }),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                CreateByteArrayContent(new byte[] { 0x11, 0x13 }, MediaTypeNames.Application.Octet)
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostByteArrayOctectBody(new byte[] { 0x10, 0x21, 0x22 }),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                CreateByteArrayContent(new byte[] { 0x10, 0x21, 0x22 }, MediaTypeNames.Application.Octet)
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostByteArrayPlainTextBody(new byte[0]),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                CreateByteArrayContent(new byte[0], MediaTypeNames.Text.Plain)
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostByteArrayPlainTextBody(new byte[] { 0x11, 0x13 }),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                CreateByteArrayContent(new byte[] { 0x11, 0x13 }, MediaTypeNames.Text.Plain)
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostByteArrayPlainTextBody(new byte[] { 0x10, 0x21, 0x22 }),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                CreateByteArrayContent(new byte[] { 0x10, 0x21, 0x22 }, MediaTypeNames.Text.Plain)
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostEmptyJsonBody(),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                CreateByteArrayContent(new byte[0], MediaTypeNames.Application.Json)
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostNameJsonBody("John", "Smith"),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                JsonContent.Create(new { FirstName = "John", LastName = "Smith" }, new MediaTypeHeaderValue(MediaTypeNames.Application.Json))
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostNameJsonBody("Jane", "Doe"),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                JsonContent.Create(new { FirstName = "Jane", LastName = "Doe" }, new MediaTypeHeaderValue(MediaTypeNames.Application.Json))
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostNameJsonPatchBody("John", "Smith"),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                JsonContent.Create(new { FirstName = "John", LastName = "Smith" }, new MediaTypeHeaderValue("application/json+patch"))
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PostNameJsonPatchBody("Jane", "Doe"),
                HttpMethod.Post,
                "5.2-preview.1",
                "/post",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(string.Empty),
                string.Empty,
                JsonContent.Create(new { FirstName = "Jane", LastName = "Doe" }, new MediaTypeHeaderValue("application/json+patch"))
            )
        }
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}