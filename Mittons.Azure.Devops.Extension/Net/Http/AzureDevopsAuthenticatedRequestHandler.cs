namespace Mittons.Azure.Devops.Extension.Net.Http;

public class AzureDevopsAuthenticatedRequestHandler : DelegatingHandler
{
    private readonly IResourceAreaUriResolver _resolver;

    public AzureDevopsAuthenticatedRequestHandler(IResourceAreaUriResolver resolver)
    {
        _resolver = resolver;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return base.SendAsync(request, cancellationToken);
    }
}
