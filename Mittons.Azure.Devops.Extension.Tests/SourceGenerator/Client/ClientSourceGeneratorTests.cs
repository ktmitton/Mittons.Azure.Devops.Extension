using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Mittons.Azure.Devops.Extension.Attributes;
using Mittons.Azure.Devops.Extension.Client;
using Mittons.Azure.Devops.Extension.Net.Http;
using Mittons.Azure.Devops.Extension.Sdk;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

[GenerateClient(ResourceAreaId.Git)]
public interface ITestGitClient
{
    [ClientRequest("5.2-preview.1", "GET", "/get")]
    Task<string> GetWithApiVersion1();

    [ClientRequest("7.3", "GET", "/get")]
    Task<string> GetWithApiVersion2();

    [ClientRequest("5.2-preview.1", "GET", "/get", "application/json")]
    Task<string> GetWithExplicitJsonMediaType();

    [ClientRequest("5.2-preview.1", "GET", "/get", "text/plain")]
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

    [ClientRequest("5.2-preview.1", "GET", "/get", "text/plain")]
    Task<string> PlainTextResponse();
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
    T ExpectedReturnValue);

public class ClientSourceGeneratorTests
{
    public class ImplementationTests
    {
        private static FunctionDefinition<string> GetWithApiVersion1 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithApiVersion1(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithApiVersion2 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithApiVersion2(),
            HttpMethod.Get,
            "7.3",
            "/get",
            string.Empty,
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithExplicitJsonMediaType = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithExplicitJsonMediaType(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithExplicitPlainTextMediaType = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithExplicitPlainTextMediaType(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "text/plain",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithPath1 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithPath1(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithPath2 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithPath2(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/path",
            string.Empty,
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithRouteParameters1_1 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters1(1, "test"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get/1/test",
            string.Empty,
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithRouteParameters1_2 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters1(274, "random"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get/274/random",
            string.Empty,
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithRouteParameters2_1 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters2(new Guid("6b22e4b8-e4c5-40ce-92ee-6e07aab08675"), 2.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/6b22e4b8-e4c5-40ce-92ee-6e07aab08675/get/2.0",
            string.Empty,
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithRouteParameters2_2 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithRouteParameters2(new Guid("15bbb737-a3c4-4065-8725-9121ad809913"), 152.37m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/15bbb737-a3c4-4065-8725-9121ad809913/get/152.37",
            string.Empty,
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters1_1 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters1("Test"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?someParameter=Test",
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters1_2 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters1("other"),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?someParameter=other",
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_1 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(true, "test", 1.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=test&testParameter1=1.0&testParameter2=true",
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_2 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(false, "other", 152.73m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=other&testParameter1=152.73&testParameter2=false",
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_3 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(default, "test", 1.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=test&testParameter1=1.0",
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_4 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(true, default, 1.0m),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?testParameter1=1.0&testParameter2=true",
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> GetWithQueryParameters2_5 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.GetWithQueryParameters2(true, "test", default),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            "?otherParameter=test&testParameter2=true",
            "application/json",
            default,
            default
        );

        private static FunctionDefinition<string> PlainTextResponse1 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.PlainTextResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "text/plain",
            new StringContent("Sample Text"),
            "Sample Text"
        );

        private static FunctionDefinition<string> PlainTextResponse2 = new FunctionDefinition<string>(
            (ITestGitClient client) => client.PlainTextResponse(),
            HttpMethod.Get,
            "5.2-preview.1",
            "/get",
            string.Empty,
            "text/plain",
            new StringContent("Here's some sample data for testing"),
            "Here's some sample data for testing"
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

        [Theory]
        [MemberData(nameof(MediaTypeTests))]
        public async Task SendAsync_WhenCalled_ExpectTheMediaTypeToBeSet<T>(FunctionDefinition<T> functionDefinition)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage();

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

        // [Fact]
        // public async Task SendAsync_WhenImplementingFunctionsnWithNoParameters_ExpectARequestToBeSentWithDefaultMediaTypeParameters()
        // {
        //     // Arrange
        //     var httpResponseMessage = new HttpResponseMessage();
        //     var expectedRequestUri = new Uri("https://localhost/test/get/url");
        //     var expectedHttpMethod = HttpMethod.Get;
        //     var expectedHeaderValues = new NameValueHeaderValue[]
        //     {
        //         new NameValueHeaderValue("excludeUrls", "true"),
        //         new NameValueHeaderValue("enumsAsNumbers", "true"),
        //         new NameValueHeaderValue("msDateFormat", "true"),
        //         new NameValueHeaderValue("noArrayWrap", "true")
        //     };

        //     var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
        //     mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
        //         .Returns(new Uri("https://localhost"));

        //     var mockSdk = new Mock<ISdk>();
        //     mockSdk.SetupGet(x => x.AuthenticationHeader)
        //         .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

        //     ServiceCollection serviceCollection = new ServiceCollection();
        //     serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
        //     serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
        //     serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

        //     using var provider = serviceCollection.BuildServiceProvider();

        //     var client = provider.GetRequiredService<ITestGitClient>();

        //     // Act
        //     await client.BasicGetTest1Async();

        //     // Assert
        //     foreach(var expectedHeaderValue in expectedHeaderValues)
        //     {
        //         Assert.Contains(expectedHeaderValue, httpResponseMessage.RequestMessage?.Headers.Accept.Single().Parameters);
        //     }
        //     Assert.Equal(expectedRequestUri, httpResponseMessage.RequestMessage?.RequestUri);
        //     Assert.Equal(expectedHttpMethod, httpResponseMessage.RequestMessage?.Method);
        //     // Assert.Contains(expectedAcceptHeader, httpResponseMessage.RequestMessage?.Headers.Accept);
        // }

        // [Fact]
        // public async Task BasicGetTest2Async_WhenImplementingAGetFunctionWithNoParameters_ExpectARequestToBeSent()
        // {
        //     // Arrange
        //     var httpResponseMessage = new HttpResponseMessage();
        //     var expectedRequestUri = new Uri("https://localhost/get");
        //     var expectedHttpMethod = HttpMethod.Get;
        //     var expectedAcceptHeader = new MediaTypeWithQualityHeaderValue("application/xml");
        //     expectedAcceptHeader.Parameters.Add(new NameValueHeaderValue("api-version", "5.2-preview.2"));
        //     expectedAcceptHeader.Parameters.Add(new NameValueHeaderValue("excludeUrls", "true"));
        //     expectedAcceptHeader.Parameters.Add(new NameValueHeaderValue("enumsAsNumbers", "true"));
        //     expectedAcceptHeader.Parameters.Add(new NameValueHeaderValue("msDateFormat", "true"));
        //     expectedAcceptHeader.Parameters.Add(new NameValueHeaderValue("noArrayWrap", "true"));

        //     var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
        //     mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
        //         .Returns(new Uri("https://localhost"));

        //     var mockSdk = new Mock<ISdk>();
        //     mockSdk.SetupGet(x => x.AuthenticationHeader)
        //         .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

        //     ServiceCollection serviceCollection = new ServiceCollection();
        //     serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
        //     serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
        //     serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

        //     using var provider = serviceCollection.BuildServiceProvider();

        //     var client = provider.GetRequiredService<ITestGitClient>();

        //     // Act
        //     await client.BasicGetTest2Async();

        //     // Assert
        //     Assert.Equal(expectedRequestUri, httpResponseMessage.RequestMessage?.RequestUri);
        //     Assert.Equal(expectedHttpMethod, httpResponseMessage.RequestMessage?.Method);
        //     Assert.Contains(expectedAcceptHeader, httpResponseMessage.RequestMessage?.Headers.Accept);
        // }

        // [Fact]
        // public async Task BasicGetTest3Async_WhenImplementingAGetFunctionWithNoParameters_ExpectARequestToBeSent()
        // {
        //     // Arrange
        //     var httpResponseMessage = new HttpResponseMessage();
        //     var expectedRequestUri = new Uri("https://localhost/get");
        //     var expectedHttpMethod = HttpMethod.Get;
        //     var expectedAcceptHeader = new MediaTypeWithQualityHeaderValue("application/json");
        //     expectedAcceptHeader.Parameters.Add(new NameValueHeaderValue("api-version", "5.2-preview.3"));
        //     expectedAcceptHeader.Parameters.Add(new NameValueHeaderValue("excludeUrls", "true"));
        //     expectedAcceptHeader.Parameters.Add(new NameValueHeaderValue("enumsAsNumbers", "true"));
        //     expectedAcceptHeader.Parameters.Add(new NameValueHeaderValue("msDateFormat", "true"));
        //     expectedAcceptHeader.Parameters.Add(new NameValueHeaderValue("noArrayWrap", "true"));

        //     var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
        //     mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
        //         .Returns(new Uri("https://localhost"));

        //     var mockSdk = new Mock<ISdk>();
        //     mockSdk.SetupGet(x => x.AuthenticationHeader)
        //         .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

        //     ServiceCollection serviceCollection = new ServiceCollection();
        //     serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
        //     serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
        //     serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

        //     using var provider = serviceCollection.BuildServiceProvider();

        //     var client = provider.GetRequiredService<ITestGitClient>();

        //     // Act
        //     await client.BasicGetTest3Async();

        //     // Assert
        //     Assert.Equal(expectedRequestUri, httpResponseMessage.RequestMessage?.RequestUri);
        //     Assert.Equal(expectedHttpMethod, httpResponseMessage.RequestMessage?.Method);
        //     Assert.Contains(expectedAcceptHeader, httpResponseMessage.RequestMessage?.Headers.Accept);
        // }
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
