using System.IO.Compression;
using System.Net.Http.Headers;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Mittons.Azure.Devops.Extension.Client;
using Mittons.Azure.Devops.Extension.Net.Http;
using Mittons.Azure.Devops.Extension.Sdk;
using Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client.TestDataGenerators;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

public class ClientSourceGeneratorTests
{
    public class ImplementationTests
    {
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
        [ClassData(typeof(ByteArrayResultTestDataGenerator))]
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
        [ClassData(typeof(RequestBodyTestDataGenerator))]
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
