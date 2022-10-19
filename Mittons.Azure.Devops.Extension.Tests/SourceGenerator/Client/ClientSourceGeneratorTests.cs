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
internal interface ITestGitClient
{
}

[GenerateClient(ResourceAreaId.Accounts)]
internal interface ITestAccountsClient
{
}

public class ClientSourceGeneratorTests
{
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

            foreach(var currentAction in configurationActions)
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

            foreach(var currentAction in configurationActions)
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

            foreach(var currentAction in configurationActions)
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
