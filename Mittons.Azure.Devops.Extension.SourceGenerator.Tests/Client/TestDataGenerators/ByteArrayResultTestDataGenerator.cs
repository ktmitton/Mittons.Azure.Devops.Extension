using System.Collections;
using System.Net.Mime;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client.TestDataGenerators;

public class ByteArrayResultTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.PlainTextByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Text.Plain,
                new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
                new byte[] { 0x26, 0x73, 0x99 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.PlainTextByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Text.Plain,
                new ByteArrayContent(new byte[] { 0x55 }),
                new byte[] { 0x55 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.PlainTextByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Text.Plain,
                new ByteArrayContent(new byte[0]),
                new byte[0],
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.ZipByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Zip,
                new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
                new byte[] { 0x26, 0x73, 0x99 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.ZipByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Zip,
                new ByteArrayContent(new byte[] { 0x55 }),
                new byte[] { 0x55 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.ZipByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Zip,
                new ByteArrayContent(new byte[0]),
                new byte[0],
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.OctetResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Octet,
                new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
                new byte[] { 0x26, 0x73, 0x99 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.OctetResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Octet,
                new ByteArrayContent(new byte[] { 0x55 }),
                new byte[] { 0x55 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.OctetResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Octet,
                new ByteArrayContent(new byte[0]),
                new byte[0],
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.SvgByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/svg+xml",
                new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
                new byte[] { 0x26, 0x73, 0x99 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.SvgByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/svg+xml",
                new ByteArrayContent(new byte[] { 0x55 }),
                new byte[] { 0x55 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.SvgByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/svg+xml",
                new ByteArrayContent(new byte[0]),
                new byte[0],
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.XamlByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/xaml+xml",
                new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
                new byte[] { 0x26, 0x73, 0x99 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.XamlByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/xaml+xml",
                new ByteArrayContent(new byte[] { 0x55 }),
                new byte[] { 0x55 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.XamlByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                "image/xaml+xml",
                new ByteArrayContent(new byte[0]),
                new byte[0],
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.XmlByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
                new byte[] { 0x26, 0x73, 0x99 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.XmlByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                new ByteArrayContent(new byte[] { 0x55 }),
                new byte[] { 0x55 },
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<byte[]>(
                (ITestGitClient client) => client.XmlByteArrayResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Xml,
                new ByteArrayContent(new byte[0]),
                new byte[0],
                default
            )
        }
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}