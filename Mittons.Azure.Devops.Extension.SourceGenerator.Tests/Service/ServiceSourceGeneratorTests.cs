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
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 2
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 4
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 5
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
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 2
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 4
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 5
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
        public async Task RemoteProxyMethodAsync_WhenTheChannelIsCalledForSimpleProxyFunction_ExpectTheCorrectFunctionIdToBeUsed(int expectedFunctionId)
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
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 2
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 4
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 5
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
        public async Task RemoteProxyMethodAsync_WhenTheChannelIsCalledForRemoteProxyFunction_ExpectTheCorrectFunctionIdToBeUsed(int expectedFunctionId)
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
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 2
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 4
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 5
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

        [Theory]
        [InlineData(1, "test", false)]
        [InlineData(2, "other", true)]
        public async Task RemoteProxyMethodAsync_WhenTheChannelIsCalledForSimpleFunctionWithArguments_ExpectAllParametersToBeSet(int expectedA, string expectedB, bool expectedOther)
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
                        FunctionId = 2
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 3
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 4
                    },
                    new Extension.SourceGenerator.Service.RemoteProxyFunctionDefinition
                    {
                        ChannelId = 1,
                        FunctionId = 5
                    }
                ));

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IChannel>(mockChannel.Object);
            serviceCollection.AddTestServiceOne();

            using var provider = serviceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService<ITestServiceOne>();

            // Act
            await service.SimpleFunctionWithArgumentsAsync(expectedA, expectedB, expectedOther, default);

            // Assert
            mockChannel.Verify(x => x.InvokeRemoteProxyMethodVoidAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), expectedA, expectedB, expectedOther), Times.Once);
        }
    }

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
