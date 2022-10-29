namespace Mittons.Azure.Devops.Extension.Sdk
{
    public interface IHostDetails
    {
        /// <summary>
        /// Unique GUID for this host
        /// </summary>
        string? Id { get; }

        /// <summary>
        /// Name of the host (i.e. Organization name)
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// Version of Azure DevOps used by the current host (organization)
        /// </summary>
        string? ServiceVersion { get; }

        /// <summary>
        /// DevOps host level
        /// </summary>
        HostType? HostType { get; }
    }
}
