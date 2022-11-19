using Microsoft.Extensions.Options;
using Mittons.Azure.Devops.Extension.Sdk;

namespace Mittons.Azure.Devops.Extension.Api.Options;

public class ConfigureNamedClientOptions : IConfigureNamedOptions<IClientOptions>
{
    private readonly ISdk _sdk;

    private readonly IResourceAreaUriResolver _resourceAreaUriResolver;

    public ConfigureNamedClientOptions(ISdk sdk, IResourceAreaUriResolver resourceAreaUriResolver)
    {
        _sdk = sdk;
        _resourceAreaUriResolver = resourceAreaUriResolver;
    }

    public void Configure(string name, IClientOptions options)
    {
        options.AuthenticationHeaderValue = _sdk.AuthenticationHeaderValue;

        var baseAddress = _resourceAreaUriResolver.Resolve(name);

        if (baseAddress is not null)
        {
            options.BaseAddress = baseAddress;
        }
    }

    public void Configure(IClientOptions options)
    {
        throw new NotImplementedException();
    }
}
