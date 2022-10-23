using System.Collections;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Xml.Serialization;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

public class DeserializedResultTestDataGenerator : IEnumerable<object[]>
{
    private static HttpContent CreateXmlContent<T>(T data)
    {
        using var stream = new MemoryStream();

        var xmlSerializer = new XmlSerializer(typeof(T));

        xmlSerializer.Serialize(stream, data);

        return new ByteArrayContent(stream.ToArray());
    }

    private readonly List<object[]> _data = new List<object[]>
    {
        new object[]
        {
            new FunctionDefinition<NameTestModel>(
                (ITestGitClient client) => client.JsonNameModelResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(new NameTestModel(new Guid("c3d95621-c52f-434a-8b02-e7d4908a40e8"), "John", "Smith")),
                new NameTestModel(new Guid("c3d95621-c52f-434a-8b02-e7d4908a40e8"), "John", "Smith"),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<NameTestModel>(
                (ITestGitClient client) => client.JsonNameModelResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(new NameTestModel(new Guid("9c7a7796-d271-4a0d-934d-4d8863aa8c43"), "Jane", "Doe")),
                new NameTestModel(new Guid("9c7a7796-d271-4a0d-934d-4d8863aa8c43"), "Jane", "Doe"),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<AddressTestModel>(
                (ITestGitClient client) => client.JsonAddressModelResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Json,
                JsonContent.Create(new AddressTestModel(1, "2 Front Street", "C/O Source Generator", "Test Town", "CA", "US")),
                new AddressTestModel(1, "2 Front Street", "C/O Source Generator", "Test Town", "CA", "US"),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<AddressTestModel>(
                (ITestGitClient client) => client.JsonAddressModelResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Zip,
                JsonContent.Create(new AddressTestModel(13, "13 Cornelia Street", String.Empty, "New York", "NY", "US")),
                new AddressTestModel(13, "13 Cornelia Street", String.Empty, "New York", "NY", "US"),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<NameTestModel>(
                (ITestGitClient client) => client.XmlNameModelResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                CreateXmlContent(new NameTestModel(new Guid("c3d95621-c52f-434a-8b02-e7d4908a40e8"), "John", "Smith")),
                new NameTestModel(new Guid("c3d95621-c52f-434a-8b02-e7d4908a40e8"), "John", "Smith"),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<NameTestModel>(
                (ITestGitClient client) => client.XmlNameModelResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                CreateXmlContent(new NameTestModel(new Guid("9c7a7796-d271-4a0d-934d-4d8863aa8c43"), "Jane", "Doe")),
                new NameTestModel(new Guid("9c7a7796-d271-4a0d-934d-4d8863aa8c43"), "Jane", "Doe"),
                default
            )
        }
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}