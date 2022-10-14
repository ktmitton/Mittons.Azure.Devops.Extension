namespace Mittons.Azure.Devops.Extension.Tests.Net.Http;

public class AzureDevopsAuthenticatedRequestHandlerTests
{
    [Fact]
    public async Task SendAsync_WhenTheResourceAreaIsUnknown_ExpectAnExceptionToBeThrown()
    {
        // Arrange
        var handler = new HttpMessageHandler();
        var invoker = new HttpMessageInvoker();

        // Act
        // Assert
    }
}
