using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Mittons.Azure.Devops.Extension.Api.Net.Http;
using Mittons.Azure.Devops.Extension.Sdk;

namespace Temp;

public interface IApiClientOptions
{
    Uri BaseAddress { get; set; }

    AuthenticationHeaderValue? AuthenticationHeaderValue { get; set; }
}

public class ConfigureNamedApiClientOptions : IConfigureNamedOptions<IApiClientOptions>
{
    private readonly ISdk _sdk;

    private readonly IResourceAreaUriResolver _resourceAreaUriResolver;

    public ConfigureNamedApiClientOptions(ISdk sdk, IResourceAreaUriResolver resourceAreaUriResolver)
    {
        _sdk = sdk;
        _resourceAreaUriResolver = resourceAreaUriResolver;
    }

    public void Configure(string name, IApiClientOptions options)
    {
        options.BaseAddress = _resourceAreaUriResolver.Resolve(name);
        options.AuthenticationHeaderValue = _sdk.AuthenticationHeader;
    }

    public void Configure(IApiClientOptions options)
        => Configure(string.Empty, options);
}
