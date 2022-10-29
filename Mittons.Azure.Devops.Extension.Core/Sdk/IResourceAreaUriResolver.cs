using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mittons.Azure.Devops.Extension.Sdk
{
    public interface IResourceAreaUriResolver
    {
        Task<Uri> ResolveAsync(string? resourceAreaId, CancellationToken cancellationToken);

        Uri Resolve(string? resourceAreaId);

        Task PrimeKnownResourceAreasAsync(CancellationToken cancellationToken);
    }
}
