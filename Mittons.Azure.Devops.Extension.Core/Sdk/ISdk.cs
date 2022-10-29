using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Mittons.Azure.Devops.Extension.Sdk
{
    public interface ISdk
    {
        string? ContributionId { get; }

        IExtensionDetails? ExtensionDetails { get; }

        IUserDetails? UserDetails { get; }

        IHostDetails? HostDetails { get; }

        Dictionary<string, string>? ThemeData { get; }

        Dictionary<string, object>? InitialConfiguration { get; }

        AuthenticationHeaderValue? AuthenticationHeaderValue { get; }

        Task InitializeAsync(decimal sdkVersion = 3.0m, bool isLoaded = true, bool applyTheme = true, CancellationToken cancellationToken = default);
    }
}
