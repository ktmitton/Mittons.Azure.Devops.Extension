using System.IO.Compression;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Mittons.Azure.Devops.Extension.Attributes;
using Mittons.Azure.Devops.Extension.Client;
using Mittons.Azure.Devops.Extension.Net.Http;
using Mittons.Azure.Devops.Extension.Sdk;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

public record NameTestModel
{
    public Guid UniqueIdentifier { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public NameTestModel() { }

    public NameTestModel(Guid uniqueIdentifier, string firstName, string lastName)
    {
        UniqueIdentifier = uniqueIdentifier;
        FirstName = firstName;
        LastName = lastName;
    }
}

public record AddressTestModel
{
    public int Id { get; set; }

    public string? Line1 { get; set; }

    public string? Line2 { get; set; }

    public string? City { get; set; }

    public string? StateCode { get; set; }

    public string? CountryCode { get; set; }

    public AddressTestModel() { }

    public AddressTestModel(int id, string line1, string line2, string city, string stateCode, string countryCode)
    {
        Id = id;
        Line1 = line1;
        Line2 = line2;
        City = city;
        StateCode = stateCode;
        CountryCode = countryCode;
    }
}

[GenerateClient(ResourceAreaId.Git)]
public interface ITestGitClient
{
    [ClientRequest("5.2-preview.1", "GET", "/get")]
    Task<string> GetWithApiVersion1();

    [ClientRequest("7.3", "GET", "/get")]
    Task<string> GetWithApiVersion2();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Json)]
    Task<string> GetWithExplicitJsonMediaType();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Plain)]
    Task<string> GetWithExplicitPlainTextMediaType();

    [ClientRequest("5.2-preview.1", "GET", "/get")]
    Task<string> GetWithPath1();

    [ClientRequest("5.2-preview.1", "GET", "/path")]
    Task<string> GetWithPath2();

    [ClientRequest("5.2-preview.1", "GET", "/get/{routeParam1}/{routeParam2}")]
    Task<string> GetWithRouteParameters1(int routeParam1, string routeParam2);

    [ClientRequest("5.2-preview.1", "GET", "/{firstRouteParam}/get/{secondRouteParam}")]
    Task<string> GetWithRouteParameters2(Guid firstRouteParam, decimal secondRouteParam);

    [ClientRequest("5.2-preview.1", "GET", "/get")]
    Task<string> GetWithQueryParameters1([ClientRequestQueryParameter] string someParameter);

    [ClientRequest("5.2-preview.1", "GET", "/get")]
    Task<string> GetWithQueryParameters2([ClientRequestQueryParameter] bool? testParameter2, [ClientRequestQueryParameter] string? otherParameter, [ClientRequestQueryParameter] decimal? testParameter1);

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Plain)]
    Task<string> PlainTextStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Plain)]
    Task<byte[]> PlainTextByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Json)]
    Task<NameTestModel> JsonNameModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Json)]
    Task<AddressTestModel> JsonAddressModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Zip)]
    Task<byte[]> ZipByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Zip)]
    Task<ZipArchive> ZipArchiveResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Octet)]
    Task<byte[]> OctetResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Html)]
    Task<byte[]> HtmlByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Html)]
    Task<string> HtmlStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/svg+xml")]
    Task<byte[]> SvgByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/svg+xml")]
    Task<string> SvgStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/svg+xml")]
    Task<XmlDocument> SvgXmlDocumentResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/xaml+xml")]
    Task<byte[]> XamlByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/xaml+xml")]
    Task<string> XamlStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/xaml+xml")]
    Task<XmlDocument> XamlXmlDocumentResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<byte[]> XmlByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<string> XmlStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<XmlDocument> XmlDocumentResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<NameTestModel> XmlNameModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<AddressTestModel> XmlAddressModelResponse();

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json)]
    Task<string> PostEmptyBody();

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json)]
    Task<string> PostByteArrayBody([ClientByteArrayRequestBodyAttribute] byte[] requestBody);

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json)]
    Task<string> PostByteArrayOctectBody([ClientByteArrayRequestBodyAttribute(MediaTypeNames.Application.Octet)] byte[] requestBody);

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json)]
    Task<string> PostByteArrayPlainTextBody([ClientByteArrayRequestBodyAttribute(MediaTypeNames.Text.Plain)] byte[] requestBody);

    // Task<string> PostJsonBody();
    // [ClientRequest("5.2-preview.2", "POST", "/test/post/url")]
    // Task<string> BasicPostTestAsync();

    // [ClientRequest("5.2-preview.1", "POST", "{projectId}/_apis/git/repositories/{repositoryId}/annotatedTags/")]
    // Task<GitAnnotatedTag> PostTestAsync([ClientRequestBody] GitAnnotatedTag tagObject, Guid projectId, Guid repositoryId);

    // [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs/{sha1}")]
    // Task<GitBlobReference> QueryParametersTestAsync(Guid projectId, Guid repositoryId, string sha1, [ClientRequestQueryParameter] bool? download, [ClientRequestQueryParameter] string? fileName, [ClientRequestQueryParameter] bool? resolveLfs);

    // [ClientRequest("5.2-preview.1", "POST", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs", "application/zip")]
    // Task<byte[]> PostWithQueryParametersTestAsync(Guid projectId, Guid repositoryId, [ClientRequestQueryParameter] string? filename, [ClientRequestBody] string[] blobIds);
}

[GenerateClient(ResourceAreaId.Accounts)]
public interface ITestAccountsClient
{
}

public class TestMessageHandler : DelegatingHandler
{
    private HttpResponseMessage _httpResponseMessage;

    public TestMessageHandler(HttpResponseMessage httpResponseMessage)
    {
        _httpResponseMessage = httpResponseMessage;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _httpResponseMessage.RequestMessage = request;

        return Task.FromResult(_httpResponseMessage);
    }
}

public record FunctionDefinition<T>(
    Func<ITestGitClient, Task<T>> TestRequestAsync,
    HttpMethod ExpectedHttpMethod,
    string ExpectedApiVersion,
    string ExpectedPath,
    string ExpectedQuery,
    string ExpectedMediaType,
    HttpContent ResponseContent,
    T ExpectedReturnValue,
    HttpContent? ExpectedRequestContent);

public class ClientSourceGeneratorTests
{
    public class ImplementationTests
    {
        public static ZipArchive CreateZipArchive(Dictionary<string, string> files)
        {
            var byteArray = CreateZipArchiveByteArray(files);
            var memoryStream = new MemoryStream();
            memoryStream.Write(byteArray, 0, byteArray.Length);

            var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read, false);

            return archive;
        }

        public static byte[] CreateZipArchiveByteArray(Dictionary<string, string> files)
        {
            using var memoryStream = new MemoryStream();

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    var entry = archive.CreateEntry(file.Key);

                    using (var entryStream = entry.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write(file.Value);
                    }
                }
            }

            return memoryStream.ToArray();
        }

        public static XmlDocument CreateXmlDocument(string contents)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(contents);

            return xmlDocument;
        }

        public static HttpContent CreateXmlContent<T>(T data)
        {
            using var stream = new MemoryStream();

            var xmlSerializer = new XmlSerializer(typeof(T));

            xmlSerializer.Serialize(stream, data);

            return new ByteArrayContent(stream.ToArray());
        }

        public static HttpContent CreateByteArrayContent(byte[] content, string mediaType)
        {
            var httpContent = new ByteArrayContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            return httpContent;
        }

        private static FunctionDefinition<string> GetWithApiVersion1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithApiVersion1(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithApiVersion2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithApiVersion2(),
            HttpMethod.Get,
            "7.3",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithExplicitJsonMediaType => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithExplicitJsonMediaType(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithExplicitPlainTextMediaType => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithExplicitPlainTextMediaType(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Text.Plain,
            new StringContent("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithPath1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithPath1(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithPath2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithPath2(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/path",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithRouteParameters1_1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters1(1, "test"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get/1/test",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithRouteParameters1_2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters1(274, "random"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get/274/random",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithRouteParameters2_1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters2(new Guid("6b22e4b8-e4c5-40ce-92ee-6e07aab08675"), 2.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/6b22e4b8-e4c5-40ce-92ee-6e07aab08675/get/2.0",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithRouteParameters2_2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters2(new Guid("15bbb737-a3c4-4065-8725-9121ad809913"), 152.37m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/15bbb737-a3c4-4065-8725-9121ad809913/get/152.37",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters1_1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters1("Test"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?someParameter=Test",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters1_2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters1("other"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?someParameter=other",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(true, "test", 1.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=test&testParameter1=1.0&testParameter2=true",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(false, "other", 152.73m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=other&testParameter1=152.73&testParameter2=false",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_3 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(default, "test", 1.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=test&testParameter1=1.0",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_4 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(true, default, 1.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?testParameter1=1.0&testParameter2=true",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_5 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(true, "test", default),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=test&testParameter2=true",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test",
            default
        );

        private static FunctionDefinition<string> PlainTextStringResponse1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PlainTextStringResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Text.Plain,
            new StringContent("Sample Text"),
            "Sample Text",
            default
        );

        private static FunctionDefinition<string> PlainTextStringResponse2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PlainTextStringResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Text.Plain,
            new StringContent("Here's some sample data for testing"),
            "Here's some sample data for testing",
            default
        );

        public static FunctionDefinition<byte[]> PlainTextByteArrayResponse1 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.PlainTextByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Text.Plain,
            new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
            new byte[] { 0x26, 0x73, 0x99 },
            default
        );

        public static FunctionDefinition<byte[]> PlainTextByteArrayResponse2 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.PlainTextByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Text.Plain,
            new ByteArrayContent(new byte[] { 0x55 }),
            new byte[] { 0x55 },
            default
        );

        public static FunctionDefinition<byte[]> PlainTextByteArrayResponse3 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.PlainTextByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Text.Plain,
            new ByteArrayContent(new byte[0]),
            new byte[0],
            default
        );

        private static FunctionDefinition<NameTestModel> JsonResponse1_1 => new FunctionDefinition<NameTestModel>(
            (ITestGitClient client) => client.JsonNameModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(new NameTestModel(new Guid("c3d95621-c52f-434a-8b02-e7d4908a40e8"), "John", "Smith")),
            new NameTestModel(new Guid("c3d95621-c52f-434a-8b02-e7d4908a40e8"), "John", "Smith"),
            default
        );

        private static FunctionDefinition<NameTestModel> JsonResponse1_2 => new FunctionDefinition<NameTestModel>(
            (ITestGitClient client) => client.JsonNameModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(new NameTestModel(new Guid("9c7a7796-d271-4a0d-934d-4d8863aa8c43"), "Jane", "Doe")),
            new NameTestModel(new Guid("9c7a7796-d271-4a0d-934d-4d8863aa8c43"), "Jane", "Doe"),
            default
        );

        private static FunctionDefinition<AddressTestModel> JsonResponse2_1 => new FunctionDefinition<AddressTestModel>(
            (ITestGitClient client) => client.JsonAddressModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(new AddressTestModel(1, "2 Front Street", "C/O Source Generator", "Test Town", "CA", "US")),
            new AddressTestModel(1, "2 Front Street", "C/O Source Generator", "Test Town", "CA", "US"),
            default
        );

        private static FunctionDefinition<AddressTestModel> JsonResponse2_2 => new FunctionDefinition<AddressTestModel>(
            (ITestGitClient client) => client.JsonAddressModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            JsonContent.Create(new AddressTestModel(13, "13 Cornelia Street", String.Empty, "New York", "NY", "US")),
            new AddressTestModel(13, "13 Cornelia Street", String.Empty, "New York", "NY", "US"),
            default
        );

        private static FunctionDefinition<byte[]> ZipByteArrayResponse1 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.ZipByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
            new byte[] { 0x26, 0x73, 0x99 },
            default
        );

        private static FunctionDefinition<byte[]> ZipByteArrayResponse2 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.ZipByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(new byte[] { 0x55 }),
            new byte[] { 0x55 },
            default
        );

        private static FunctionDefinition<byte[]> ZipByteArrayResponse3 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.ZipByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(new byte[0]),
            new byte[0],
            default
        );

        private static FunctionDefinition<ZipArchive> ZipArchiveResponse1 => new FunctionDefinition<ZipArchive>(
            (ITestGitClient client) => client.ZipArchiveResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(CreateZipArchiveByteArray(new Dictionary<string, string> { { "text.txt", "Test Content" } })),
            CreateZipArchive(new Dictionary<string, string> { { "text.txt", "Test Content" } }),
            default
        );

        private static FunctionDefinition<ZipArchive> ZipArchiveResponse2 => new FunctionDefinition<ZipArchive>(
            (ITestGitClient client) => client.ZipArchiveResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(CreateZipArchiveByteArray(new Dictionary<string, string> { { "test.csv", "Not really a csv" }, { "test.pdf", "Also not really a pdf" } })),
            CreateZipArchive(new Dictionary<string, string> { { "test.csv", "Not really a csv" }, { "test.pdf", "Also not really a pdf" } }),
            default
        );

        private static FunctionDefinition<ZipArchive> ZipArchiveResponse3 => new FunctionDefinition<ZipArchive>(
            (ITestGitClient client) => client.ZipArchiveResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(CreateZipArchiveByteArray(new Dictionary<string, string>())),
            CreateZipArchive(new Dictionary<string, string>()),
            default
        );

        private static FunctionDefinition<byte[]> OctetyResponse1 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.OctetResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Octet,
            new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
            new byte[] { 0x26, 0x73, 0x99 },
            default
        );

        private static FunctionDefinition<byte[]> OctetyResponse2 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.OctetResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Octet,
            new ByteArrayContent(new byte[] { 0x55 }),
            new byte[] { 0x55 },
            default
        );

        private static FunctionDefinition<byte[]> OctetyResponse3 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.OctetResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Octet,
            new ByteArrayContent(new byte[0]),
            new byte[0],
            default
        );

        private static FunctionDefinition<string> SvgStringResponse1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.SvgStringResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/svg+xml",
            new StringContent("Sample Text"),
            "Sample Text",
            default
        );

        private static FunctionDefinition<string> SvgStringResponse2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.SvgStringResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/svg+xml",
            new StringContent("Here's some sample data for testing"),
            "Here's some sample data for testing",
            default
        );

        public static FunctionDefinition<byte[]> SvgByteArrayResponse1 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.SvgByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/svg+xml",
            new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
            new byte[] { 0x26, 0x73, 0x99 },
            default
        );

        public static FunctionDefinition<byte[]> SvgByteArrayResponse2 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.SvgByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/svg+xml",
            new ByteArrayContent(new byte[] { 0x55 }),
            new byte[] { 0x55 },
            default
        );

        public static FunctionDefinition<byte[]> SvgByteArrayResponse3 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.SvgByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/svg+xml",
            new ByteArrayContent(new byte[0]),
            new byte[0],
            default
        );

        public static FunctionDefinition<XmlDocument> SvgXmlDocumentResponse1 => new FunctionDefinition<XmlDocument>(
            (ITestGitClient client) => client.SvgXmlDocumentResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/svg+xml",
            new StringContent("<svg><line /></svg>"),
            CreateXmlDocument("<svg><line /></svg>"),
            default
        );

        public static FunctionDefinition<XmlDocument> SvgXmlDocumentResponse2 => new FunctionDefinition<XmlDocument>(
            (ITestGitClient client) => client.SvgXmlDocumentResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/svg+xml",
            new StringContent("<myxml></myxml>"),
            CreateXmlDocument("<myxml></myxml>"),
            default
        );

        private static FunctionDefinition<string> XamlStringResponse1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.XamlStringResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/xaml+xml",
            new StringContent("Sample Text"),
            "Sample Text",
            default
        );

        private static FunctionDefinition<string> XamlStringResponse2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.XamlStringResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/xaml+xml",
            new StringContent("Here's some sample data for testing"),
            "Here's some sample data for testing",
            default
        );

        public static FunctionDefinition<byte[]> XamlByteArrayResponse1 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.XamlByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/xaml+xml",
            new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
            new byte[] { 0x26, 0x73, 0x99 },
            default
        );

        public static FunctionDefinition<byte[]> XamlByteArrayResponse2 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.XamlByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/xaml+xml",
            new ByteArrayContent(new byte[] { 0x55 }),
            new byte[] { 0x55 },
            default
        );

        public static FunctionDefinition<byte[]> XamlByteArrayResponse3 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.XamlByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/xaml+xml",
            new ByteArrayContent(new byte[0]),
            new byte[0],
            default
        );

        public static FunctionDefinition<XmlDocument> XamlXmlDocumentResponse1 => new FunctionDefinition<XmlDocument>(
            (ITestGitClient client) => client.XamlXmlDocumentResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/xaml+xml",
            new StringContent("<svg><line /></svg>"),
            CreateXmlDocument("<svg><line /></svg>"),
            default
        );

        public static FunctionDefinition<XmlDocument> XamlXmlDocumentResponse2 => new FunctionDefinition<XmlDocument>(
            (ITestGitClient client) => client.XamlXmlDocumentResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "image/xaml+xml",
            new StringContent("<myxml></myxml>"),
            CreateXmlDocument("<myxml></myxml>"),
            default
        );

        private static FunctionDefinition<string> XmlStringResponse1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.XmlStringResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            new StringContent("Sample Text"),
            "Sample Text",
            default
        );

        private static FunctionDefinition<string> XmlStringResponse2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.XmlStringResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            new StringContent("Here's some sample data for testing"),
            "Here's some sample data for testing",
            default
        );

        public static FunctionDefinition<byte[]> XmlByteArrayResponse1 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.XmlByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
            new byte[] { 0x26, 0x73, 0x99 },
            default
        );

        public static FunctionDefinition<byte[]> XmlByteArrayResponse2 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.XmlByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            new ByteArrayContent(new byte[] { 0x55 }),
            new byte[] { 0x55 },
            default
        );

        public static FunctionDefinition<byte[]> XmlByteArrayResponse3 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.XmlByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            new ByteArrayContent(new byte[0]),
            new byte[0],
            default
        );

        public static FunctionDefinition<XmlDocument> XmlXmlDocumentResponse1 => new FunctionDefinition<XmlDocument>(
            (ITestGitClient client) => client.XmlDocumentResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            new StringContent("<svg><line /></svg>"),
            CreateXmlDocument("<svg><line /></svg>"),
            default
        );

        public static FunctionDefinition<XmlDocument> XmlXmlDocumentResponse2 => new FunctionDefinition<XmlDocument>(
            (ITestGitClient client) => client.XmlDocumentResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            new StringContent("<myxml></myxml>"),
            CreateXmlDocument("<myxml></myxml>"),
            default
        );

        private static FunctionDefinition<NameTestModel> XmlResponse1_1 => new FunctionDefinition<NameTestModel>(
            (ITestGitClient client) => client.XmlNameModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            CreateXmlContent(new NameTestModel(new Guid("c3d95621-c52f-434a-8b02-e7d4908a40e8"), "John", "Smith")),
            new NameTestModel(new Guid("c3d95621-c52f-434a-8b02-e7d4908a40e8"), "John", "Smith"),
            default
        );

        private static FunctionDefinition<NameTestModel> XmlResponse1_2 => new FunctionDefinition<NameTestModel>(
            (ITestGitClient client) => client.XmlNameModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            CreateXmlContent(new NameTestModel(new Guid("9c7a7796-d271-4a0d-934d-4d8863aa8c43"), "Jane", "Doe")),
            new NameTestModel(new Guid("9c7a7796-d271-4a0d-934d-4d8863aa8c43"), "Jane", "Doe"),
            default
        );

        private static FunctionDefinition<AddressTestModel> XmlResponse2_1 => new FunctionDefinition<AddressTestModel>(
            (ITestGitClient client) => client.XmlAddressModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            JsonContent.Create(new AddressTestModel(1, "2 Front Street", "C/O Source Generator", "Test Town", "CA", "US")),
            new AddressTestModel(1, "2 Front Street", "C/O Source Generator", "Test Town", "CA", "US"),
            default
        );

        private static FunctionDefinition<AddressTestModel> XmlResponse2_2 => new FunctionDefinition<AddressTestModel>(
            (ITestGitClient client) => client.XmlAddressModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Xml,
            JsonContent.Create(new AddressTestModel(13, "13 Cornelia Street", String.Empty, "New York", "NY", "US")),
            new AddressTestModel(13, "13 Cornelia Street", String.Empty, "New York", "NY", "US"),
            default
        );

        private static FunctionDefinition<string> PostEmptyBody => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostEmptyBody(),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            default
        );

        private static FunctionDefinition<string> PostByteArrayBody1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostByteArrayBody(new byte[0]),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            CreateByteArrayContent(new byte[0], MediaTypeNames.Application.Octet)
        );

        private static FunctionDefinition<string> PostByteArrayBody2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostByteArrayBody(new byte[] { 0x11, 0x13 }),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            CreateByteArrayContent(new byte[] { 0x11, 0x13 }, MediaTypeNames.Application.Octet)
        );

        private static FunctionDefinition<string> PostByteArrayBody3 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostByteArrayBody(new byte[] { 0x10, 0x21, 0x22 }),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            CreateByteArrayContent(new byte[] { 0x10, 0x21, 0x22 }, MediaTypeNames.Application.Octet)
        );

        private static FunctionDefinition<string> PostByteArrayOctectBody1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostByteArrayOctectBody(new byte[0]),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            CreateByteArrayContent(new byte[0], MediaTypeNames.Application.Octet)
        );

        private static FunctionDefinition<string> PostByteArrayOctectBody2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostByteArrayOctectBody(new byte[] { 0x11, 0x13 }),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            CreateByteArrayContent(new byte[] { 0x11, 0x13 }, MediaTypeNames.Application.Octet)
        );

        private static FunctionDefinition<string> PostByteArrayOctectBody3 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostByteArrayOctectBody(new byte[] { 0x10, 0x21, 0x22 }),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            CreateByteArrayContent(new byte[] { 0x10, 0x21, 0x22 }, MediaTypeNames.Application.Octet)
        );

        private static FunctionDefinition<string> PostByteArrayPlainTextBody1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostByteArrayPlainTextBody(new byte[0]),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            CreateByteArrayContent(new byte[0], MediaTypeNames.Text.Plain)
        );

        private static FunctionDefinition<string> PostByteArrayPlainTextBody2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostByteArrayPlainTextBody(new byte[] { 0x11, 0x13 }),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            CreateByteArrayContent(new byte[] { 0x11, 0x13 }, MediaTypeNames.Text.Plain)
        );

        private static FunctionDefinition<string> PostByteArrayPlainTextBody3 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostByteArrayPlainTextBody(new byte[] { 0x10, 0x21, 0x22 }),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            CreateByteArrayContent(new byte[] { 0x10, 0x21, 0x22 }, MediaTypeNames.Text.Plain)
        );

        internal static IEnumerable<object[]> HttpMethodTests()
        {
            yield return new object[] { GetWithApiVersion1 };
            yield return new object[] { PostEmptyBody };
        }

        internal static IEnumerable<object[]> MediaTypeTests()
        {
            yield return new object[] { GetWithExplicitJsonMediaType };
            yield return new object[] { GetWithExplicitPlainTextMediaType };
        }

        internal static IEnumerable<object[]> MediaTypeParameterTests()
        {
            yield return new object[] { GetWithApiVersion1 };
            yield return new object[] { GetWithApiVersion2 };
            yield return new object[] { GetWithExplicitJsonMediaType };
            yield return new object[] { GetWithExplicitPlainTextMediaType };
            yield return new object[] { GetWithPath1 };
            yield return new object[] { GetWithPath2 };
            yield return new object[] { GetWithRouteParameters1_1 };
            yield return new object[] { GetWithRouteParameters1_2 };
            yield return new object[] { GetWithRouteParameters2_1 };
            yield return new object[] { GetWithRouteParameters2_2 };
            yield return new object[] { GetWithQueryParameters1_1 };
            yield return new object[] { GetWithQueryParameters1_2 };
            yield return new object[] { GetWithQueryParameters2_1 };
            yield return new object[] { GetWithQueryParameters2_2 };
            yield return new object[] { GetWithQueryParameters2_3 };
            yield return new object[] { GetWithQueryParameters2_4 };
            yield return new object[] { GetWithQueryParameters2_5 };
        }

        internal static IEnumerable<object[]> ApiVersionTests()
        {
            yield return new object[] { GetWithApiVersion1 };
            yield return new object[] { GetWithApiVersion2 };
        }

        internal static IEnumerable<object[]> BasicPathTests()
        {
            yield return new object[] { GetWithPath1 };
            yield return new object[] { GetWithPath2 };
        }

        internal static IEnumerable<object[]> RouteParameterTests()
        {
            yield return new object[] { GetWithRouteParameters1_1 };
            yield return new object[] { GetWithRouteParameters1_2 };
            yield return new object[] { GetWithRouteParameters2_1 };
            yield return new object[] { GetWithRouteParameters2_2 };
        }

        internal static IEnumerable<object[]> QueryParameterTests()
        {
            yield return new object[] { GetWithRouteParameters2_2 };
            yield return new object[] { GetWithQueryParameters1_1 };
            yield return new object[] { GetWithQueryParameters1_2 };
            yield return new object[] { GetWithQueryParameters2_1 };
            yield return new object[] { GetWithQueryParameters2_2 };
            yield return new object[] { GetWithQueryParameters2_3 };
            yield return new object[] { GetWithQueryParameters2_4 };
            yield return new object[] { GetWithQueryParameters2_5 };
        }

        internal static IEnumerable<object[]> ByteArrayResultTests()
        {
            yield return new object[] { PlainTextByteArrayResponse1 };
            yield return new object[] { PlainTextByteArrayResponse2 };
            yield return new object[] { PlainTextByteArrayResponse3 };
            yield return new object[] { ZipByteArrayResponse1 };
            yield return new object[] { ZipByteArrayResponse2 };
            yield return new object[] { ZipByteArrayResponse3 };
            yield return new object[] { OctetyResponse1 };
            yield return new object[] { OctetyResponse2 };
            yield return new object[] { OctetyResponse3 };
            yield return new object[] { SvgByteArrayResponse1 };
            yield return new object[] { SvgByteArrayResponse2 };
            yield return new object[] { SvgByteArrayResponse3 };
            yield return new object[] { XamlByteArrayResponse1 };
            yield return new object[] { XamlByteArrayResponse2 };
            yield return new object[] { XamlByteArrayResponse3 };
            yield return new object[] { XmlByteArrayResponse1 };
            yield return new object[] { XmlByteArrayResponse2 };
            yield return new object[] { XmlByteArrayResponse3 };
        }

        internal static IEnumerable<object[]> JsonResultTests()
        {
            yield return new object[] { JsonResponse1_1 };
            yield return new object[] { JsonResponse1_2 };
            yield return new object[] { JsonResponse2_1 };
            yield return new object[] { JsonResponse2_2 };
            yield return new object[] { XmlResponse1_1 };
            yield return new object[] { XmlResponse1_2 };
        }

        internal static IEnumerable<object[]> StringResultTests()
        {
            yield return new object[] { PlainTextStringResponse1 };
            yield return new object[] { PlainTextStringResponse2 };
            yield return new object[] { SvgStringResponse1 };
            yield return new object[] { SvgStringResponse2 };
            yield return new object[] { XamlStringResponse1 };
            yield return new object[] { XamlStringResponse2 };
            yield return new object[] { XmlStringResponse1 };
            yield return new object[] { XmlStringResponse2 };
        }

        internal static IEnumerable<object[]> XmlResultTests()
        {
            yield return new object[] { SvgXmlDocumentResponse1 };
            yield return new object[] { SvgXmlDocumentResponse2 };
            yield return new object[] { XamlXmlDocumentResponse1 };
            yield return new object[] { XamlXmlDocumentResponse2 };
            yield return new object[] { XmlXmlDocumentResponse1 };
            yield return new object[] { XmlXmlDocumentResponse2 };
        }

        internal static IEnumerable<object[]> ZipArchiveResultTests()
        {
            yield return new object[] { ZipArchiveResponse1 };
            yield return new object[] { ZipArchiveResponse2 };
            yield return new object[] { ZipArchiveResponse3 };
        }

        internal static IEnumerable<object[]> RequestBodyTests()
        {
            yield return new object[] { PostEmptyBody };
            yield return new object[] { PostByteArrayBody1 };
            yield return new object[] { PostByteArrayBody2 };
            yield return new object[] { PostByteArrayBody3 };
            yield return new object[] { PostByteArrayOctectBody1 };
            yield return new object[] { PostByteArrayOctectBody2 };
            yield return new object[] { PostByteArrayOctectBody3 };
            yield return new object[] { PostByteArrayPlainTextBody1 };
            yield return new object[] { PostByteArrayPlainTextBody2 };
            yield return new object[] { PostByteArrayPlainTextBody3 };
        }

        [Theory]
        [MemberData(nameof(HttpMethodTests))]
        public async Task SendAsync_WhenCalled_ExpectTheHttpMethodToBeSet<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            await functionDefinition.TestRequestAsync(client);

            // Assert
            Assert.Equal(functionDefinition.ExpectedHttpMethod, httpResponseMessage.RequestMessage?.Method);
        }

        [Theory]
        [MemberData(nameof(MediaTypeTests))]
        public async Task SendAsync_WhenCalled_ExpectTheMediaTypeToBeSet<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            await functionDefinition.TestRequestAsync(client);

            // Assert
            var acceptHeader = httpResponseMessage.RequestMessage?.Headers.Accept.Single();

            Assert.Equal(functionDefinition.ExpectedMediaType, acceptHeader?.MediaType);
        }

        [Theory]
        [MemberData(nameof(MediaTypeParameterTests))]
        public async Task SendAsync_WhenCalled_ExpectTheDefaultMediaTypeParametersToBeSet<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            var expectedHeaderValues = new NameValueHeaderValue[]
            {
                new NameValueHeaderValue("excludeUrls", "true"),
                new NameValueHeaderValue("enumsAsNumbers", "true"),
                new NameValueHeaderValue("msDateFormat", "true"),
                new NameValueHeaderValue("noArrayWrap", "true")
            };

            // Act
            await functionDefinition.TestRequestAsync(client);

            // Assert
            var acceptHeader = httpResponseMessage.RequestMessage?.Headers.Accept.Single();

            foreach (var expectedHeaderValue in expectedHeaderValues)
            {
                Assert.Contains(expectedHeaderValue, acceptHeader?.Parameters);
            }
        }

        [Theory]
        [MemberData(nameof(ApiVersionTests))]
        public async Task SendAsync_WhenCalled_ExpectTheMediaTypeApiVersionParameterToBeSet<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            var expectedHeaderValue = new NameValueHeaderValue("api-version", functionDefinition.ExpectedApiVersion);

            // Act
            await functionDefinition.TestRequestAsync(client);

            // Assert
            var acceptHeader = httpResponseMessage.RequestMessage?.Headers.Accept.Single();

            Assert.Contains(expectedHeaderValue, acceptHeader?.Parameters);
        }

        [Theory]
        [MemberData(nameof(BasicPathTests))]
        public async Task SendAsync_WhenCalledWithSimplePaths_ExpectThePathToBeSet<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            await functionDefinition.TestRequestAsync(client);

            // Assert
            Assert.Equal(functionDefinition.ExpectedPath, httpResponseMessage.RequestMessage?.RequestUri?.AbsolutePath);
        }

        [Theory]
        [MemberData(nameof(RouteParameterTests))]
        public async Task SendAsync_WhenCalledWithParameterizedRoutes_ExpectThePathToBeSet<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            await functionDefinition.TestRequestAsync(client);

            // Assert
            Assert.Equal(functionDefinition.ExpectedPath, httpResponseMessage.RequestMessage?.RequestUri?.AbsolutePath);
        }

        [Theory]
        [MemberData(nameof(QueryParameterTests))]
        public async Task SendAsync_WhenCalledWithParameterizedQueries_ExpectTheQueryToBeSet<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            await functionDefinition.TestRequestAsync(client);

            // Assert
            Assert.Equal(functionDefinition.ExpectedQuery, httpResponseMessage.RequestMessage?.RequestUri?.Query);
        }

        [Theory]
        [MemberData(nameof(ByteArrayResultTests))]
        public async Task SendAsync_WhenCallingAnEndpointWithAByteArrayResult_ExpectTheResponseContentToBeReturned<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            var actualResult = await functionDefinition.TestRequestAsync(client);

            // Assert
            Assert.Equal(functionDefinition.ExpectedReturnValue, actualResult);
        }

        [Theory]
        [MemberData(nameof(JsonResultTests))]
        public async Task SendAsync_WhenCallingAJsonEndpoint_ExpectTheResponseContentToBeReturned<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            var actualResult = await functionDefinition.TestRequestAsync(client);

            // Assert
            Assert.Equal(functionDefinition.ExpectedReturnValue, actualResult);
        }

        [Theory]
        [MemberData(nameof(StringResultTests))]
        public async Task SendAsync_WhenCallingAnEndpointWithAStringResult_ExpectTheResponseContentToBeReturned<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            var actualResult = await functionDefinition.TestRequestAsync(client);

            // Assert
            Assert.Equal(functionDefinition.ExpectedReturnValue, actualResult);
        }

        [Theory]
        [MemberData(nameof(XmlResultTests))]
        public async Task SendAsync_WhenCallingAnEndpointWithAnXmlResult_ExpectTheResponseContentToBeReturned(FunctionDefinition<XmlDocument> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            var actualResult = await functionDefinition.TestRequestAsync(client);

            // Assert
            Assert.Equal(functionDefinition.ExpectedReturnValue, actualResult);
        }

        [Theory]
        [MemberData(nameof(ZipArchiveResultTests))]
        public async Task SendAsync_WhenCallingAZipEndpointWithADisposableResult_ExpectTheResponseContentToBeReturned(FunctionDefinition<ZipArchive> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            using var actualResult = await functionDefinition.TestRequestAsync(client);

            // Assert
            var actualDetails = actualResult.Entries.OrderBy(x => x.FullName).Select(x => new
            {
                FullName = x.FullName,
                Length = x.Length,
                CompressedLength = x.CompressedLength,
                Crc32 = x.Crc32
            });

            var expectedDetails = functionDefinition.ExpectedReturnValue.Entries.OrderBy(x => x.FullName).Select(x => new
            {
                FullName = x.FullName,
                Length = x.Length,
                CompressedLength = x.CompressedLength,
                Crc32 = x.Crc32
            });

            Assert.Equal(expectedDetails, actualDetails);
        }

        [Theory]
        [MemberData(nameof(RequestBodyTests))]
        public async Task SendAsync_WhenCalled_ExpectTheRequestBodyToBeSet(FunctionDefinition<string> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = functionDefinition.ResponseContent;

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
            serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

            using var provider = serviceCollection.BuildServiceProvider();

            var client = provider.GetRequiredService<ITestGitClient>();

            // Act
            await functionDefinition.TestRequestAsync(client);

            // Assert
            if (functionDefinition.ExpectedRequestContent is null)
            {
                Assert.Null(httpResponseMessage.RequestMessage?.Content);
            }
            else
            {
                Assert.Equal(functionDefinition.ExpectedRequestContent.Headers.ContentType, httpResponseMessage.RequestMessage?.Content?.Headers.ContentType);
                Assert.Equal(await functionDefinition.ExpectedRequestContent.ReadAsByteArrayAsync(), await httpResponseMessage.RequestMessage!.Content!.ReadAsByteArrayAsync());
            }
        }
    }

    public class ExtensionsTests
    {
        [Theory]
        [InlineData(typeof(ITestGitClient), ResourceAreaId.Git, "https://localhost/first")]
        [InlineData(typeof(ITestGitClient), ResourceAreaId.Git, "https://localhost/second")]
        [InlineData(typeof(ITestAccountsClient), ResourceAreaId.Accounts, "https://localhost/first")]
        [InlineData(typeof(ITestAccountsClient), ResourceAreaId.Accounts, "https://localhost/second")]
        public void AddClient_WhenAClientIsResolved_ExpectTheBaseAddressToBeSet(Type clientType, string resourceAreaId, string url)
        {
            // Arrange
            var expectedBaseAddress = new Uri(url);

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(resourceAreaId))
                .Returns(expectedBaseAddress);

            var mockSdk = new Mock<ISdk>();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);

            // Act
            serviceCollection.AddTestGitClient();
            serviceCollection.AddTestAccountsClient();

            using var provider = serviceCollection.BuildServiceProvider();
            var configurationActions = provider.GetRequiredService<IOptionsMonitor<HttpClientFactoryOptions>>().Get(clientType.Name).HttpClientActions;

            var actualHttpClient = new HttpClient();

            foreach (var currentAction in configurationActions)
            {
                currentAction(actualHttpClient);
            }

            // Assert
            Assert.Equal(expectedBaseAddress, actualHttpClient.BaseAddress);
        }

        [Theory]
        [InlineData(typeof(ITestGitClient))]
        [InlineData(typeof(ITestAccountsClient))]
        public void AddClient_WhenAClientIsResolved_ExpectTheDefaultAuthorizationToBeSet(Type clientType)
        {
            // Arrange
            var expectedAuthenticationHeader = new AuthenticationHeaderValue("Scheme", "Parameter");

            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(expectedAuthenticationHeader);

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);

            // Act
            serviceCollection.AddTestGitClient();
            serviceCollection.AddTestAccountsClient();

            using var provider = serviceCollection.BuildServiceProvider();
            var configurationActions = provider.GetRequiredService<IOptionsMonitor<HttpClientFactoryOptions>>().Get(clientType.Name).HttpClientActions;

            var actualHttpClient = new HttpClient();

            foreach (var currentAction in configurationActions)
            {
                currentAction(actualHttpClient);
            }

            // Assert
            Assert.Equal(expectedAuthenticationHeader, actualHttpClient.DefaultRequestHeaders.Authorization);
        }

        [Theory]
        [InlineData(typeof(ITestGitClient))]
        [InlineData(typeof(ITestAccountsClient))]
        public void AddClient_WhenAClientIsResolved_ExpectTheDefaultHeadersToBeSet(Type clientType)
        {
            // Arrange
            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);

            // Act
            serviceCollection.AddTestGitClient();
            serviceCollection.AddTestAccountsClient();

            using var provider = serviceCollection.BuildServiceProvider();
            var configurationActions = provider.GetRequiredService<IOptionsMonitor<HttpClientFactoryOptions>>().Get(clientType.Name).HttpClientActions;

            var actualHttpClient = new HttpClient();

            foreach (var currentAction in configurationActions)
            {
                currentAction(actualHttpClient);
            }

            // Assert
            Assert.Equal(new[] { "Suppress" }, actualHttpClient.DefaultRequestHeaders.GetValues("X-VSS-ReauthenticationAction"));
            Assert.Equal(new[] { "Suppress" }, actualHttpClient.DefaultRequestHeaders.GetValues("X-TFS-FedAuthRedirect"));
        }

        [Theory]
        [InlineData(typeof(ITestGitClient))]
        [InlineData(typeof(ITestAccountsClient))]
        public void AddTestClient_WhenAllDependenciesAreRegistered_ExpectAClientToBeResolved(Type clientType)
        {
            // Arrange
            var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
            mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
                .Returns(new Uri("https://localhost"));

            var mockSdk = new Mock<ISdk>();
            mockSdk.SetupGet(x => x.AuthenticationHeader)
                .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
            serviceCollection.AddSingleton<ISdk>(mockSdk.Object);

            // Act
            serviceCollection.AddTestGitClient();
            serviceCollection.AddTestAccountsClient();

            // Assert
            using (var provider = serviceCollection.BuildServiceProvider())
            {
                var result = provider.GetRequiredService(clientType);
                Assert.NotNull(result);
            }
        }
    }
}
