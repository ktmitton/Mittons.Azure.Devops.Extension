using System.Net.Http.Headers;
using Mittons.Azure.Devops.Extension.Api.Options;
using Mittons.Azure.Devops.Extension.Sdk;

namespace Mittons.Azure.Devops.Extension.Api.Tests.Options;

public class ConfigureNamedClientOptionsTests
{
    [Fact]
    public void Configure_WhenCalledWithAName_ExpectTheAuthenticationHeaderToBeSet()
    {
        // Arrange
        var expectedAuthenticationHeaderValue = new AuthenticationHeaderValue(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        var mockSdk = new Mock<ISdk>();
        mockSdk.SetupGet(x => x.AuthenticationHeaderValue).Returns(expectedAuthenticationHeaderValue);

        var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();

        var mockOptions = new Mock<IClientOptions>();

        var configure = new ConfigureNamedClientOptions(mockSdk.Object, mockResourceAreaUriResolver.Object);

        // Act
        configure.Configure(string.Empty, mockOptions.Object);

        // Assert
        mockOptions.VerifySet(x => x.AuthenticationHeaderValue = expectedAuthenticationHeaderValue, Times.Once);
    }

    [Theory]
    [InlineData("", "https://localhost")]
    [InlineData("test", "https://www.example.com")]
    public void Configure_WhenCalledWithAValidName_ExpectTheBaseAddressToBeSet(string resourceAreaId, string url)
    {
        // Arrange
        var expectedUri = new Uri(url);

        var mockSdk = new Mock<ISdk>();

        var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();
        mockResourceAreaUriResolver.Setup(x => x.Resolve(resourceAreaId)).Returns(expectedUri);

        var mockOptions = new Mock<IClientOptions>();

        var configure = new ConfigureNamedClientOptions(mockSdk.Object, mockResourceAreaUriResolver.Object);

        // Act
        configure.Configure(resourceAreaId, mockOptions.Object);

        // Assert
        mockOptions.VerifySet(x => x.BaseAddress = expectedUri, Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("test")]
    public void Configure_WhenCalledWithAnInvalidName_ExpectTheBaseAddressToNotBeSet(string resourceAreaId)
    {
        // Arrange
        var mockSdk = new Mock<ISdk>();

        var mockResourceAreaUriResolver = new Mock<IResourceAreaUriResolver>();

        var mockOptions = new Mock<IClientOptions>();

        var configure = new ConfigureNamedClientOptions(mockSdk.Object, mockResourceAreaUriResolver.Object);

        // Act
        configure.Configure(resourceAreaId, mockOptions.Object);

        // Assert
        mockOptions.VerifySet(x => x.BaseAddress = It.IsAny<Uri>(), Times.Never);
    }
}
