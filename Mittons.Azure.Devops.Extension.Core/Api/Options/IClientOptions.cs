using System;
using System.Net.Http.Headers;

namespace Mittons.Azure.Devops.Extension.Api.Options
{
    public interface IClientOptions
    {
        Uri BaseAddress { get; set; }

        AuthenticationHeaderValue? AuthenticationHeaderValue { get; set; }
    }
}
