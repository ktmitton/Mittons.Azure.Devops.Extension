namespace Mittons.Azure.Devops.Extension.Net.Http;

public interface IResourceAreaUriResolver
{
    Task<Uri> Resolve(string resourceAreaId);
}

public class AzureDevopsAuthenticatedRequestHandler : DelegatingHandler
{
    private readonly IResourceAreaUriResolver _resolver;

    public AzureDevopsAuthenticatedRequestHandler(IResourceAreaUriResolver resolver)
    {
        _resolver = resolver;c#
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return base.SendAsync(request, cancellationToken);
    }
}
