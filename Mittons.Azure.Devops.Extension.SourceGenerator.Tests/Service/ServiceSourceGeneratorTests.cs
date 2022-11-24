using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Mittons.Azure.Devops.Extension.Sdk;
using Mittons.Azure.Devops.Extension.Sdk.Xdm;
// using Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client.TestDataGenerators;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Service;

public class ServiceSourceGeneratorTests
{
    public class ImplementationTests
    {
        [Fact]
        public async Task RemoteProxyMethodAsync_WhenTheChannelIsCalled_ExpectTheCancellationTokenToBePassed()
        {
            // Arrange
            var expectedCancellationToken = new CancellationTokenSource().Token;

            var mockChannel = new Mock<IChannel>();
            mockChannel.Setup(x => x.GetServiceDefinitionAsync<TestServiceOneRemoteProxyFunctionDefinitionCollection>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TestServiceOneRemoteProxyFunctionDefinitionCollection
                (
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 1
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 2
                    }
                ));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IChannel>(mockChannel.Object);
            serviceCollection.AddTestServiceOne();

            using var provider = serviceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService<ITestServiceOne>();

            // Act
            await service.SimpleFunctionAsync(expectedCancellationToken);

            // Assert
            mockChannel.Verify(x => x.InvokeRemoteProxyMethodVoidAsync(It.IsAny<int>(), expectedCancellationToken), Times.Once);
        }

        [Fact]
        public async Task RemoteProxyMethodAsync_WhenTheRemoteFunctionsAreRetrievedForServiceOne_ExpectTheServiceOneContributionIdToBeUsed()
        {
            // Arrange
            var expectedContributionId = "mitt.test-service-one";

            var mockChannel = new Mock<IChannel>();
            mockChannel.Setup(x => x.GetServiceDefinitionAsync<TestServiceOneRemoteProxyFunctionDefinitionCollection>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TestServiceOneRemoteProxyFunctionDefinitionCollection
                (
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 1
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 2
                    }
                ));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IChannel>(mockChannel.Object);
            serviceCollection.AddTestServiceOne();

            using var provider = serviceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService<ITestServiceOne>();

            // Act
            await service.SimpleFunctionAsync(default);

            // Assert
            mockChannel.Verify(x => x.GetServiceDefinitionAsync<TestServiceOneRemoteProxyFunctionDefinitionCollection>(expectedContributionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RemoteProxyMethodAsync_WhenTheRemoteFunctionsAreRetrievedForServiceTwo_ExpectTheServiceTwoContributionIdToBeUsed()
        {
            // Arrange
            var expectedContributionId = "two";

            var mockChannel = new Mock<IChannel>();
            mockChannel.Setup(x => x.GetServiceDefinitionAsync<TestServiceTwoRemoteProxyFunctionDefinitionCollection>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TestServiceTwoRemoteProxyFunctionDefinitionCollection
                (
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 1
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 2
                    }
                ));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IChannel>(mockChannel.Object);
            serviceCollection.AddTestServiceTwo();

            using var provider = serviceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService<ITestServiceTwo>();

            // Act
            await service.SimpleFunctionAsync(default);

            // Assert
            mockChannel.Verify(x => x.GetServiceDefinitionAsync<TestServiceTwoRemoteProxyFunctionDefinitionCollection>(expectedContributionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task SimpleFunctionAsync_WhenTheChannelIsCalled_ExpectTheCorrectFunctionIdToBeUsed(int expectedFunctionId)
        {
            // Arrange
            var mockChannel = new Mock<IChannel>();
            mockChannel.Setup(x => x.GetServiceDefinitionAsync<TestServiceOneRemoteProxyFunctionDefinitionCollection>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TestServiceOneRemoteProxyFunctionDefinitionCollection
                (
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = expectedFunctionId
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 2
                    }
                ));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IChannel>(mockChannel.Object);
            serviceCollection.AddTestServiceOne();

            using var provider = serviceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService<ITestServiceOne>();

            // Act
            await service.SimpleFunctionAsync(default);

            // Assert
            mockChannel.Verify(x => x.InvokeRemoteProxyMethodVoidAsync(expectedFunctionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task SimpleFunctionRenamedAsync_WhenTheChannelIsCalled_ExpectTheCorrectFunctionIdToBeUsed(int expectedFunctionId)
        {
            // Arrange
            var mockChannel = new Mock<IChannel>();
            mockChannel.Setup(x => x.GetServiceDefinitionAsync<TestServiceOneRemoteProxyFunctionDefinitionCollection>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TestServiceOneRemoteProxyFunctionDefinitionCollection
                (
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 1
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = expectedFunctionId
                    }
                ));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IChannel>(mockChannel.Object);
            serviceCollection.AddTestServiceOne();

            using var provider = serviceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService<ITestServiceOne>();

            // Act
            await service.SimpleFunctionRenamedAsync(default);

            // Assert
            mockChannel.Verify(x => x.InvokeRemoteProxyMethodVoidAsync(expectedFunctionId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
    // public class ImplementationTests
    // {
    //     [Theory]
    //     [ClassData(typeof(HttpMethodTestDataGenerator))]
    //     public async Task SendAsync_WhenCalled_ExpectTheHttpMethodToBeSet<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Equal(functionDefinition.ExpectedHttpMethod, httpResponseMessage.RequestMessage?.Method);
    //     }

    //     [Theory]
    //     [ClassData(typeof(MediaTypeTestDataGenerator))]
    //     public async Task SendAsync_WhenCalled_ExpectTheMediaTypeToBeSet<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         var acceptHeader = httpResponseMessage.RequestMessage?.Headers.Accept.Single();

    //         Assert.Equal(functionDefinition.ExpectedMediaType, acceptHeader?.MediaType);
    //     }

    //     [Theory]
    //     [ClassData(typeof(MediaTypeParameterTestDataGenerator))]
    //     public async Task SendAsync_WhenCalled_ExpectTheDefaultMediaTypeParametersToBeSet<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         var expectedHeaderValues = new NameValueHeaderValue[]
    //         {
    //             new NameValueHeaderValue("excludeUrls", "true"),
    //             new NameValueHeaderValue("enumsAsNumbers", "true"),
    //             new NameValueHeaderValue("msDateFormat", "true"),
    //             new NameValueHeaderValue("noArrayWrap", "true")
    //         };

    //         // Act
    //         await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         var acceptHeader = httpResponseMessage.RequestMessage!.Headers.Accept.Single();

    //         foreach (var expectedHeaderValue in expectedHeaderValues)
    //         {
    //             Assert.Contains(expectedHeaderValue, acceptHeader.Parameters);
    //         }
    //     }

    //     [Theory]
    //     [ClassData(typeof(ApiVersionTestDataGenerator))]
    //     public async Task SendAsync_WhenCalled_ExpectTheMediaTypeApiVersionParameterToBeSet<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         var expectedHeaderValue = new NameValueHeaderValue("api-version", functionDefinition.ExpectedApiVersion);

    //         // Act
    //         await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         var acceptHeader = httpResponseMessage.RequestMessage!.Headers.Accept.Single();

    //         Assert.Contains(expectedHeaderValue, acceptHeader.Parameters);
    //     }

    //     [Theory]
    //     [ClassData(typeof(BasicPathTestDataGenerator))]
    //     public async Task SendAsync_WhenCalledWithSimplePaths_ExpectThePathToBeSet<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Equal(functionDefinition.ExpectedPath, httpResponseMessage.RequestMessage?.RequestUri?.AbsolutePath);
    //     }

    //     [Theory]
    //     [ClassData(typeof(RouteParameterTestDataGenerator))]
    //     public async Task SendAsync_WhenCalledWithParameterizedRoutes_ExpectThePathToBeSet<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Equal(functionDefinition.ExpectedPath, httpResponseMessage.RequestMessage?.RequestUri?.AbsolutePath);
    //     }

    //     [Theory]
    //     [ClassData(typeof(QueryParameterTestDataGenerator))]
    //     public async Task SendAsync_WhenCalledWithParameterizedQueries_ExpectTheQueryToBeSet<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Equal(functionDefinition.ExpectedQuery, httpResponseMessage.RequestMessage?.RequestUri?.Query);
    //     }

    //     [Theory]
    //     [ClassData(typeof(ByteArrayResultTestDataGenerator))]
    //     public async Task SendAsync_WhenCallingAnEndpointWithAByteArrayResult_ExpectTheResponseContentToBeReturned<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         var actualResult = await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Equal(functionDefinition.ExpectedReturnValue, actualResult);
    //     }

    //     [Theory]
    //     [ClassData(typeof(DeserializedResultTestDataGenerator))]
    //     public async Task SendAsync_WhenCallingAJsonEndpoint_ExpectTheResponseContentToBeReturned<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         var actualResult = await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Equal(functionDefinition.ExpectedReturnValue, actualResult);
    //     }

    //     [Theory]
    //     [ClassData(typeof(StringResultTestDataGenerator))]
    //     public async Task SendAsync_WhenCallingAnEndpointWithAStringResult_ExpectTheResponseContentToBeReturned<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         var actualResult = await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Equal(functionDefinition.ExpectedReturnValue, actualResult);
    //     }

    //     [Theory]
    //     [ClassData(typeof(XmlResultTestDataGenerator))]
    //     public async Task SendAsync_WhenCallingAnEndpointWithAnXmlResult_ExpectTheResponseContentToBeReturned(FunctionDefinition<XmlDocument> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         var actualResult = await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Equal(functionDefinition.ExpectedReturnValue, actualResult);
    //     }

    //     [Theory]
    //     [ClassData(typeof(ZipArchiveTestDataGenerator))]
    //     public async Task SendAsync_WhenCallingAZipEndpointWithADisposableResult_ExpectTheResponseContentToBeReturned(FunctionDefinition<ZipArchive> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         using var actualResult = await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         var actualDetails = actualResult.Entries.OrderBy(x => x.FullName).Select(x => new
    //         {
    //             FullName = x.FullName,
    //             Length = x.Length,
    //             CompressedLength = x.CompressedLength,
    //             Crc32 = x.Crc32
    //         });

    //         var expectedDetails = functionDefinition.ExpectedReturnValue!.Entries.OrderBy(x => x.FullName).Select(x => new
    //         {
    //             FullName = x.FullName,
    //             Length = x.Length,
    //             CompressedLength = x.CompressedLength,
    //             Crc32 = x.Crc32
    //         });

    //         Assert.Equal(expectedDetails, actualDetails);
    //     }

    //     [Theory]
    //     [ClassData(typeof(RequestBodyTestDataGenerator))]
    //     public async Task SendAsync_WhenCalled_ExpectTheRequestBodyToBeSet(FunctionDefinition<string> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = functionDefinition.ResponseContent
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         if (functionDefinition.ExpectedRequestContent is null)
    //         {
    //             Assert.Null(httpResponseMessage.RequestMessage?.Content);
    //         }
    //         else
    //         {
    //             Assert.Equal(functionDefinition.ExpectedRequestContent.Headers.ContentType, httpResponseMessage.RequestMessage?.Content?.Headers.ContentType);
    //             Assert.Equal(await functionDefinition.ExpectedRequestContent.ReadAsByteArrayAsync(), await httpResponseMessage.RequestMessage!.Content!.ReadAsByteArrayAsync());
    //         }
    //     }

    //     [Theory]
    //     [InlineData(HttpStatusCode.NotFound)]
    //     [InlineData(HttpStatusCode.GatewayTimeout)]
    //     public async Task SendAsync_WhenANonSuccessCodeIsReturned_ExpectAnExceptionToBeThrown(HttpStatusCode httpStatusCode)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage(httpStatusCode);

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         // Assert
    //         await Assert.ThrowsAsync<HttpRequestException>(() => client.GetWithApiVersion1());
    //     }

    //     [Theory]
    //     [ClassData(typeof(NullableDeserializedResultTestDataGenerator))]
    //     public async Task SendAsync_WhenAnEmptyResponseIsReturnedForANullableDeserializedResult_ExpectNullToBeReturned<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = new ByteArrayContent(new byte[0])
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         var result = await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Null(result);
    //     }

    //     [Theory]
    //     [ClassData(typeof(NonNullableInvalidDeserializedResultTestDataGenerator))]
    //     public async Task SendAsync_WhenAnEmptyResponseIsReturnedForANonNullableDeserializedResult_ExpectAnExceptionToBeReturned<T>(FunctionDefinition<T> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = new ByteArrayContent(new byte[0])
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         // Assert
    //         await Assert.ThrowsAsync<InvalidOperationException>(() => functionDefinition.TestRequestAsync(client));
    //     }

    //     [Theory]
    //     [ClassData(typeof(NonNullableEmptyStringResultTestDataGenerator))]
    //     public async Task SendAsync_WhenAnEmptyResponseIsReturnedForANonNullableStringResult_ExpectEmptyStringToBeReturned<T>(FunctionDefinition<string> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = new ByteArrayContent(new byte[0])
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         var actualResult = await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Equal(string.Empty, actualResult);
    //     }

    //     [Theory]
    //     [ClassData(typeof(NullableEmptyStringResultTestDataGenerator))]
    //     public async Task SendAsync_WhenAnEmptyResponseIsReturnedForANullableStringResult_ExpectNullToBeReturned<T>(FunctionDefinition<string?> functionDefinition)
    //     {
    //         // Arrange
    //         var httpResponseMessage = new HttpResponseMessage
    //         {
    //             Content = new ByteArrayContent(new byte[0])
    //         };

    //         var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
    //         mockResourceAreaUriResolver.Setup(x => x.Resolve(It.IsAny<string>()))
    //             .Returns(new Uri("https://localhost"));

    //         var mockSdk = new Mock<ISdk>();
    //         mockSdk.SetupGet(x => x.AuthenticationHeaderValue)
    //             .Returns(new AuthenticationHeaderValue("Scheme", "Parameter"));

    //         ServiceCollection serviceCollection = new ServiceCollection();
    //         serviceCollection.AddSingleton<IResourceAreaUriResolver>(mockResourceAreaUriResolver.Object);
    //         serviceCollection.AddSingleton<ISdk>(mockSdk.Object);
    //         serviceCollection.AddTestGitClient().AddHttpMessageHandler(() => new TestMessageHandler(httpResponseMessage));

    //         using var provider = serviceCollection.BuildServiceProvider();

    //         var client = provider.GetRequiredService<ITestGitClient>();

    //         // Act
    //         var actualResult = await functionDefinition.TestRequestAsync(client);

    //         // Assert
    //         Assert.Null(actualResult);
    //     }
    // }

    public class ExtensionsTests
    {
        [Fact]
        public void AddServiceOne_WhenServiceOneIsResolved_ExpectAValidInstanceToBeReturned()
        {
            // Arrange
            var mockChannel = new Mock<IChannel>();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IChannel>(mockChannel.Object);

            // Act
            serviceCollection.AddTestServiceOne();

            using var provider = serviceCollection.BuildServiceProvider();
            var actualInstance = provider.GetRequiredService<ITestServiceOne>();

            // Assert
            Assert.NotNull(actualInstance);
        }

        [Fact]
        public void AddServiceTwo_WhenServiceTwoIsResolved_ExpectAValidInstanceToBeReturned()
        {
            // Arrange
            var mockChannel = new Mock<IChannel>();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IChannel>(mockChannel.Object);

            // Act
            serviceCollection.AddTestServiceTwo();

            using var provider = serviceCollection.BuildServiceProvider();
            var actualInstance = provider.GetRequiredService<ITestServiceTwo>();

            // Assert
            Assert.NotNull(actualInstance);
        }
    }
}
