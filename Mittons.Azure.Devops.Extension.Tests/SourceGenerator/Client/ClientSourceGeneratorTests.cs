using System.IO.Compression;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Mittons.Azure.Devops.Extension.Attributes;
using Mittons.Azure.Devops.Extension.Client;
using Mittons.Azure.Devops.Extension.Net.Http;
using Mittons.Azure.Devops.Extension.Sdk;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

public record JsonNameTestModel(Guid UniqueIdentifier, string FirstName, string LastName);

public record JsonAddressTestModel(int Id, string Line1, string Line2, string City, string StateCode, string CountryCode);

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
    Task<string> PlainTextResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Plain)]
    Task<byte[]> PlainTextByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Json)]
    Task<JsonNameTestModel> JsonNameModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Json)]
    Task<JsonAddressTestModel> JsonAddressModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Zip)]
    Task<byte[]> ZipByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Zip)]
    Task<ZipArchive> ZipArchiveResponse();
    // Task<ZipArchive> ZipArchiveResponse();
    // [ClientRequest("5.2-preview.2", "POST", "/test/post/url")]
    // Task<string> BasicPostTestAsync();

    // [ClientRequest("5.2-preview.1", "POST", "{projectId}/_apis/git/repositories/{repositoryId}/annotatedTags/")]
    // Task<GitAnnotatedTag> PostTestAsync([ClientRequestBody] GitAnnotatedTag tagObject, Guid projectId, Guid repositoryId);

    // [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs/{sha1}")]
    // Task<GitBlobReference> QueryParametersTestAsync(Guid projectId, Guid repositoryId, string sha1, [ClientRequestQueryParameter] bool? download, [ClientRequestQueryParameter] string? fileName, [ClientRequestQueryParameter] bool? resolveLfs);

    // [ClientRequest("5.2-preview.1", "POST", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs", "application/zip")]
    // Task<byte[]> PostWithQueryParametersTestAsync(Guid projectId, Guid repositoryId, [ClientRequestQueryParameter] string? filename, [ClientRequestBody] string[] blobIds);


    // application/octet-stream
    // text/html
    // image/svg+xml
    // image/xaml+xml
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
    T ExpectedReturnValue);

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

        private static FunctionDefinition<string> GetWithApiVersion1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithApiVersion1(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithApiVersion2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithApiVersion2(),
            HttpMethod.Get,
            "7.3",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithExplicitJsonMediaType => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithExplicitJsonMediaType(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithExplicitPlainTextMediaType => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithExplicitPlainTextMediaType(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Text.Plain,
            new StringContent("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithPath1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithPath1(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithPath2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithPath2(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/path",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithRouteParameters1_1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters1(1, "test"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get/1/test",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithRouteParameters1_2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters1(274, "random"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get/274/random",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithRouteParameters2_1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters2(new Guid("6b22e4b8-e4c5-40ce-92ee-6e07aab08675"), 2.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/6b22e4b8-e4c5-40ce-92ee-6e07aab08675/get/2.0",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithRouteParameters2_2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters2(new Guid("15bbb737-a3c4-4065-8725-9121ad809913"), 152.37m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/15bbb737-a3c4-4065-8725-9121ad809913/get/152.37",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithQueryParameters1_1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters1("Test"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?someParameter=Test",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithQueryParameters1_2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters1("other"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?someParameter=other",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(true, "test", 1.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=test&testParameter1=1.0&testParameter2=true",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(false, "other", 152.73m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=other&testParameter1=152.73&testParameter2=false",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_3 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(default, "test", 1.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=test&testParameter1=1.0",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_4 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(true, default, 1.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?testParameter1=1.0&testParameter2=true",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_5 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(true, "test", default),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=test&testParameter2=true",
            MediaTypeNames.Application.Json,
            JsonContent.Create("Test"),
            "Test"
        );

        private static FunctionDefinition<string> PlainTextResponse1 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PlainTextResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Text.Plain,
            new StringContent("Sample Text"),
            "Sample Text"
        );

        private static FunctionDefinition<string> PlainTextResponse2 => new FunctionDefinition<string>(
            (ITestGitClient client) => client.PlainTextResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Text.Plain,
            new StringContent("Here's some sample data for testing"),
            "Here's some sample data for testing"
        );

        private static FunctionDefinition<JsonNameTestModel> JsonResponse1_1 => new FunctionDefinition<JsonNameTestModel>(
            (ITestGitClient client) => client.JsonNameModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(new JsonNameTestModel(new Guid("c3d95621-c52f-434a-8b02-e7d4908a40e8"), "John", "Smith")),
            new JsonNameTestModel(new Guid("c3d95621-c52f-434a-8b02-e7d4908a40e8"), "John", "Smith")
        );

        private static FunctionDefinition<JsonNameTestModel> JsonResponse1_2 => new FunctionDefinition<JsonNameTestModel>(
            (ITestGitClient client) => client.JsonNameModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(new JsonNameTestModel(new Guid("9c7a7796-d271-4a0d-934d-4d8863aa8c43"), "Jane", "Doe")),
            new JsonNameTestModel(new Guid("9c7a7796-d271-4a0d-934d-4d8863aa8c43"), "Jane", "Doe")
        );

        private static FunctionDefinition<JsonAddressTestModel> JsonResponse2_1 => new FunctionDefinition<JsonAddressTestModel>(
            (ITestGitClient client) => client.JsonAddressModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Json,
            JsonContent.Create(new JsonAddressTestModel(1, "2 Front Street", "C/O Source Generator", "Test Town", "CA", "US")),
            new JsonAddressTestModel(1, "2 Front Street", "C/O Source Generator", "Test Town", "CA", "US")
        );

        private static FunctionDefinition<JsonAddressTestModel> JsonResponse2_2 => new FunctionDefinition<JsonAddressTestModel>(
            (ITestGitClient client) => client.JsonAddressModelResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            JsonContent.Create(new JsonAddressTestModel(13, "13 Cornelia Street", String.Empty, "New York", "NY", "US")),
            new JsonAddressTestModel(13, "13 Cornelia Street", String.Empty, "New York", "NY", "US")
        );

        private static FunctionDefinition<byte[]> ZipByteArrayResponse1 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.ZipByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(new byte[] { 0x26, 0x73, 0x99 }),
            new byte[] { 0x26, 0x73, 0x99 }
        );

        private static FunctionDefinition<byte[]> ZipByteArrayResponse2 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.ZipByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(new byte[] { 0x55 }),
            new byte[] { 0x55 }
        );

        private static FunctionDefinition<byte[]> ZipByteArrayResponse3 => new FunctionDefinition<byte[]>(
            (ITestGitClient client) => client.ZipByteArrayResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(new byte[0]),
            new byte[0]
        );

        private static FunctionDefinition<ZipArchive> ZipArchiveResponse1 => new FunctionDefinition<ZipArchive>(
            (ITestGitClient client) => client.ZipArchiveResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(CreateZipArchiveByteArray(new Dictionary<string, string> { { "text.txt", "Test Content" } })),
            CreateZipArchive(new Dictionary<string, string> { { "text.txt", "Test Content" } })
        );

        private static FunctionDefinition<ZipArchive> ZipArchiveResponse2 => new FunctionDefinition<ZipArchive>(
            (ITestGitClient client) => client.ZipArchiveResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(CreateZipArchiveByteArray(new Dictionary<string, string> { { "test.csv", "Not really a csv" }, { "test.pdf", "Also not really a pdf" } })),
            CreateZipArchive(new Dictionary<string, string> { { "test.csv", "Not really a csv" }, { "test.pdf", "Also not really a pdf" } })
        );

        private static FunctionDefinition<ZipArchive> ZipArchiveResponse3 => new FunctionDefinition<ZipArchive>(
            (ITestGitClient client) => client.ZipArchiveResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            MediaTypeNames.Application.Zip,
            new ByteArrayContent(CreateZipArchiveByteArray(new Dictionary<string, string>())),
            CreateZipArchive(new Dictionary<string, string>())
        );

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

        internal static IEnumerable<object[]> PlainTextTests()
        {
            yield return new object[] { PlainTextResponse1 };
            yield return new object[] { PlainTextResponse2 };
        }

        internal static IEnumerable<object[]> JsonTests()
        {
            yield return new object[] { JsonResponse1_1 };
            yield return new object[] { JsonResponse1_2 };
            yield return new object[] { JsonResponse2_1 };
            yield return new object[] { JsonResponse2_2 };
        }

        internal static IEnumerable<object[]> ZipByteArrayTests()
        {
            yield return new object[] { ZipByteArrayResponse1 };
            yield return new object[] { ZipByteArrayResponse2 };
            yield return new object[] { ZipByteArrayResponse3 };
        }

        internal static IEnumerable<object[]> ZipArchiveTests()
        {
            yield return new object[] { ZipArchiveResponse1 };
            yield return new object[] { ZipArchiveResponse2 };
            yield return new object[] { ZipArchiveResponse3 };
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
        [MemberData(nameof(PlainTextTests))]
        public async Task SendAsync_WhenCallingAPlainTextEndpointAndReturningAValidType_ExpectTheResponseContentToBeReturned<T>(FunctionDefinition<T> functionDefinition)
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
        [MemberData(nameof(PlainTextTests))]
        public async Task SendAsync_WhenCallingAPlainTextEndpoint_ExpectAnExceptionToBeReturned<T>(FunctionDefinition<T> functionDefinition)
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
        [MemberData(nameof(JsonTests))]
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
        [MemberData(nameof(ZipByteArrayTests))]
        public async Task SendAsync_WhenCallingAZipEndpointWithANonDisposableResult_ExpectTheResponseContentToBeReturned<T>(FunctionDefinition<T> functionDefinition)
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
        [MemberData(nameof(ZipArchiveTests))]
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
