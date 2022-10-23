using System.Collections;
using System.Net.Mime;
using System.Xml;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

public class XmlResultTestDataGenerator : IEnumerable<object[]>
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
            new FunctionDefinition<XmlDocument>(
                (ITestGitClient client) => client.SvgXmlDocumentResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/svg+xml",
                new StringContent("<svg><line /></svg>"),
                CreateXmlDocument("<svg><line /></svg>"),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<XmlDocument>(
                (ITestGitClient client) => client.SvgXmlDocumentResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/svg+xml",
                new StringContent("<myxml></myxml>"),
                CreateXmlDocument("<myxml></myxml>"),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<XmlDocument>(
                (ITestGitClient client) => client.XamlXmlDocumentResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/xaml+xml",
                new StringContent("<svg><line /></svg>"),
                CreateXmlDocument("<svg><line /></svg>"),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<XmlDocument>(
                (ITestGitClient client) => client.XamlXmlDocumentResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/xaml+xml",
                new StringContent("<myxml></myxml>"),
                CreateXmlDocument("<myxml></myxml>"),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<XmlDocument>(
                (ITestGitClient client) => client.XmlDocumentResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                new StringContent("<svg><line /></svg>"),
                CreateXmlDocument("<svg><line /></svg>"),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<XmlDocument>(
                (ITestGitClient client) => client.XmlDocumentResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                new StringContent("<myxml></myxml>"),
                CreateXmlDocument("<myxml></myxml>"),
                default
            )
        }
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}