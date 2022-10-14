using Microsoft.Extensions.DependencyInjection;
using Mittons.Azure.Devops.Extension.Attributes;
using Mittons.Azure.Devops.Extension.Client;
using Mittons.Azure.Devops.Extension.Sdk;
using Moq;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

[GenerateClient(ResourceAreaId.Git)]
public interface ITestGitClient
{
}

[GenerateClient(ResourceAreaId.Accounts)]
public interface ITestAccountsClient
{
}

public class TestDelegatingHandler : DelegatingHandler
{
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return new HttpResponseMessage
        {
            RequestMessage = request
        };
    }
}

public class ClientSourceGeneratorTests
{
    [Theory]
    [InlineData(typeof(ITestGitClient), ResourceAreaId.Git)]
    [InlineData(typeof(ITestAccountsClient), ResourceAreaId.Accounts)]
    public void AddTestClient_WhenAllDependenciesAreRegistered_ExpectAClientToBeResolved(Type clientType, string expectedResourceAreaId)
    {
        // Arrange
        var a = new HttpMessageInvoker();
        var mockSdk = new Mock<ISdk>();
        mockSdk.SetupGet(x => x.ResourceAreaUris)
            .Returns(
                new Dictionary<string, Uri>
                {
                    { ResourceAreaId.Git, new Uri("https://localhost/git") },
                    { ResourceAreaId.Accounts, new Uri("https://localhost/accounts") }
                }
            );

        ServiceCollection serviceCollection = new ServiceCollection();
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

    [Fact]
    public void AddTestClient_WhenAnSdkIsNotRegistered_ExpectAnExceptionToBeThrownOnResolution()
    {
        // Arrange
        ServiceCollection serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddTestGitClient();

        // Assert
        using (var provider = serviceCollection.BuildServiceProvider())
        {
            Assert.Throws<InvalidOperationException>(() => provider.GetRequiredService<ITestGitClient>());
        }
    }

    [Fact]
    public void AddTestClient_WhenAResourceAreaIdIsNotRegistered_ExpectAnExceptionToBeThrownOnResolution()
    {
        // Arrange
        var mockSdk = new Mock<ISdk>();
        mockSdk.SetupGet(x => x.ResourceAreaUris)
            .Returns(
                new Dictionary<string, Uri>()
            );

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ISdk>(mockSdk.Object);

        // Act
        serviceCollection.AddTestGitClient();

        // Assert
        using (var provider = serviceCollection.BuildServiceProvider())
        {
            Assert.Throws<ArgumentException>(() => provider.GetRequiredService<ITestGitClient>());
        }
    }

    [Theory]
    [InlineData(typeof(ITestGitClient), ResourceAreaId.Git)]
    [InlineData(typeof(ITestAccountsClient), ResourceAreaId.Accounts)]
    public void AddTestClient_WhenAllDependenciesAreRegistered_ExpectHttpClientToHaveDefaultsSet(Type clientType, string expectedResourceAreaId)
    {
        // Arrange
        var mockSdk = new Mock<ISdk>();
        mockSdk.SetupGet(x => x.ResourceAreaUris)
            .Returns(
                new Dictionary<string, Uri>
                {
                    { ResourceAreaId.Git, new Uri("https://localhost/git") },
                    { ResourceAreaId.Accounts, new Uri("https://localhost/accounts") }
                }
            );

        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://localhost");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ISdk>(mockSdk.Object);

        // Act
        serviceCollection.AddTestGitClient().AddHttpMessageHandler<TestDelegatingHandler>();
        serviceCollection.AddTestAccountsClient().AddHttpMessageHandler<TestDelegatingHandler>();
        //serviceCollection.AddSingleton<IHttpClientFactory>(mockHttpClientFactory.Object);

        // Assert
        using (var provider = serviceCollection.BuildServiceProvider())
        {
            var result = provider.GetRequiredService(clientType);
            provider.GetRequiredService<ITestGitClient>();
            Assert.NotNull(result);
        }
    }
}
