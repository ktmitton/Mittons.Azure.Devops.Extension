using System.Net;
using Mittons.Azure.Devops.Extension.Net.Http;
using Moq;
using Moq.Protected;

namespace Mittons.Azure.Devops.Extension.Tests.Net.Http;

public class AzureDevopsAuthenticatedRequestHandlerTests
{
    [Fact]
    public async Task SendAsync_WhenTheResourceAreaIsUnknown_ExpectAnExceptionToBeThrown()
    {
        // Arrange
        var innerHandlerMock = new Mock<DelegatingHandler>(MockBehavior.Strict);
        innerHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken _) => new HttpResponseMessage { RequestMessage = request });

        var handler = new AzureDevopsAuthenticatedRequestHandler
        {
            InnerHandler = innerHandlerMock.Object
        };

        var invoker = new HttpMessageInvoker(handler);

        // Act
        var response = await invoker.SendAsync(new HttpRequestMessage(), default);

        // Assert
    }
}
