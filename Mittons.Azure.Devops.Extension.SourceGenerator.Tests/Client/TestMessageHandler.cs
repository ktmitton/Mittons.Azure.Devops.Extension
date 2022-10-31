namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

internal class TestMessageHandler : DelegatingHandler
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