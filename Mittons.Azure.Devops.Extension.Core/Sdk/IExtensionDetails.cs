namespace Mittons.Azure.Devops.Extension.Sdk
{
    public interface IExtensionDetails
    {
        /// <summary>
        /// Full id of the extension <publisher>.<extension>
        /// </summary>
        string? Id { get; }

        /// <summary>
        /// Id of the publisher
        /// </summary>
        string? PublisherId { get; }

        /// <summary>
        /// Id of the extension (without the publisher prefix)
        /// </summary>
        string? ExtensionId { get; }

        /// <summary>
        /// Version of the extension
        /// </summary>
        string? Version { get; }
    }
}
