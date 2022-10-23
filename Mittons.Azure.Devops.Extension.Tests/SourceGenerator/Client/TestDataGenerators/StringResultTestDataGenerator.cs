using System.Collections;
using System.Net.Mime;
using System.Xml;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client.TestDataGenerators;

public class StringResultTestDataGenerator : IEnumerable<object[]>
{
    private static XmlDocument CreateXmlDocument(string contents)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(contents);

        return xmlDocument;
    }

    private readonly List<object[]> _data = new List<object[]>
    {
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PlainTextStringResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Text.Plain,
                new StringContent("Sample Text"),
                "Sample Text",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.PlainTextStringResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Text.Plain,
                new StringContent("Here's some sample data for testing"),
                "Here's some sample data for testing",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.SvgStringResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/svg+xml",
                new StringContent("Sample Text"),
                "Sample Text",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.SvgStringResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/svg+xml",
                new StringContent("Here's some sample data for testing"),
                "Here's some sample data for testing",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.XamlStringResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/xaml+xml",
                new StringContent("Sample Text"),
                "Sample Text",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.XamlStringResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/xaml+xml",
                new StringContent("Here's some sample data for testing"),
                "Here's some sample data for testing",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.XmlStringResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                new StringContent("Sample Text"),
                "Sample Text",
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<string>(
                (ITestGitClient client) => client.XmlStringResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                new StringContent("Here's some sample data for testing"),
                "Here's some sample data for testing",
                default
            )
        }
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}