using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Mittons.Azure.Devops.Extension.Api.Net.Http
{
    public interface IAuthenticationHeaderResolver
    {
        Task<AuthenticationHeaderValue> ResolveAsync(CancellationToken cancellationToken);

        AuthenticationHeaderValue Resolve();
    }
}
