using System.IO.Compression;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Xml;
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

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json, MediaTypeNames.Application.Octet)]
    Task<string> PostByteArrayOctectBody([ClientByteArrayRequestBodyAttribute] byte[] requestBody);

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json, MediaTypeNames.Text.Plain)]
    Task<string> PostByteArrayPlainTextBody([ClientByteArrayRequestBodyAttribute] byte[] requestBody);

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json, MediaTypeNames.Application.Json)]
    Task<string> PostEmptyJsonBody();

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json, MediaTypeNames.Application.Json)]
    Task<string> PostNameJsonBody([ClientJsonRequestBodyParameterAttribute] string firstName, [ClientJsonRequestBodyParameterAttribute] string lastName);

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json, "application/json+patch")]
    Task<string> PostNameJsonPatchBody([ClientJsonRequestBodyParameterAttribute] string firstName, [ClientJsonRequestBodyParameterAttribute] string lastName);
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
        public static HttpContent CreateByteArrayContent(byte[] content, string mediaType)
        {
            var httpContent = new ByteArrayContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            return httpContent;
        }

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

        private static FunctionDefinition<string> PostEmptyJsonBody => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostEmptyJsonBody(),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            CreateByteArrayContent(new byte[0], MediaTypeNames.Application.Json)
        );

        private static FunctionDefinition<string> PostNameJsonBody1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostNameJsonBody("John", "Smith"),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            JsonContent.Create(new { FirstName = "John", LastName = "Smith" }, new MediaTypeHeaderValue(MediaTypeNames.Application.Json))
        );

        private static FunctionDefinition<string> PostNameJsonBody2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostNameJsonBody("Jane", "Doe"),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            JsonContent.Create(new { FirstName = "Jane", LastName = "Doe" }, new MediaTypeHeaderValue(MediaTypeNames.Application.Json))
        );

        private static FunctionDefinition<string> PostNameJsonPatchBody1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostNameJsonPatchBody("John", "Smith"),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            JsonContent.Create(new { FirstName = "John", LastName = "Smith" }, new MediaTypeHeaderValue("application/json+patch"))
        );

        private static FunctionDefinition<string> PostNameJsonPatchBody2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PostNameJsonPatchBody("Jane", "Doe"),
            HttpMethod.Post,
            "5.2-preview.1",
            "/post",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(string.Empty),
            string.Empty,
            JsonContent.Create(new { FirstName = "Jane", LastName = "Doe" }, new MediaTypeHeaderValue("application/json+patch"))
        );

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
            yield return new object[] { PostEmptyJsonBody };
            yield return new object[] { PostNameJsonBody1 };
            yield return new object[] { PostNameJsonBody2 };
            yield return new object[] { PostNameJsonPatchBody1 };
            yield return new object[] { PostNameJsonPatchBody2 };
        }

        [Theory]
        [ClassData(typeof(HttpMethodTestDataGenerator))]
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
        [ClassData(typeof(MediaTypeTestDataGenerator))]
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
        [ClassData(typeof(MediaTypeParameterTestDataGenerator))]
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
        [ClassData(typeof(ApiVersionTestDataGenerator))]
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
        [ClassData(typeof(BasicPathTestDataGenerator))]
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
        [ClassData(typeof(RouteParameterTestDataGenerator))]
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
        [ClassData(typeof(QueryParameterTestDataGenerator))]
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
        [ClassData(typeof(DeserializedResultTestDataGenerator))]
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
        [ClassData(typeof(StringResultTestDataGenerator))]
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
        [ClassData(typeof(XmlResultTestDataGenerator))]
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
        [ClassData(typeof(ZipArchiveTestDataGenerator))]
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
