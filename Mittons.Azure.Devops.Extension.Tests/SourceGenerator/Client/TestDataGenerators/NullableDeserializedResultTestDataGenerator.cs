using System.Collections;
using System.Net.Mime;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client.TestDataGenerators;

public class NullableDeserializedResultTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[]
        {
            new FunctionDefinition<NameTestModel?>(
                (ITestGitClient client) => client.JsonNullableNameModelResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Json,
                new ByteArrayContent(new byte[0]),
                default,
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<AddressTestModel?>(
                (ITestGitClient client) => client.JsonNullableAddressModelResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Json,
                new ByteArrayContent(new byte[0]),
                default,
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<NameTestModel?>(
                (ITestGitClient client) => client.XmlNullableNameModelResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                new ByteArrayContent(new byte[0]),
                default,
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<AddressTestModel?>(
                (ITestGitClient client) => client.XmlNullableAddressModelResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                new ByteArrayContent(new byte[0]),
                default,
                default
            )
        }
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}