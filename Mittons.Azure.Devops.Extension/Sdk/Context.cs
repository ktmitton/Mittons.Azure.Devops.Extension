using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk;

public class Context
{
    [JsonPropertyName("extension")]
    public ExtensionDetails? Extension { get; }

    [JsonPropertyName("user")]
    public UserDetails? User { get; }

    [JsonPropertyName("host")]
    public HostDetails? Host { get; }

    public class ExtensionDetails
    {
        /// <summary>
        /// Full id of the extension <publisher>.<extension>
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; }

        /// <summary>
        /// Id of the publisher
        /// </summary>
        [JsonPropertyName("publisherId")]
        public string? PublisherId { get; }

        /// <summary>
        /// Id of the extension (without the publisher prefix)
        /// </summary>
        [JsonPropertyName("extensionId")]
        public string? ExtensionId { get; }

        /// <summary>
        /// Version of the extension
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version { get; }
    }

    public class HostDetails
    {
        /// <summary>
        /// Unique GUID for this host
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; }

        /// <summary>
        /// Name of the host (i.e. Organization name)
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; }

        /// <summary>
        /// Version of Azure DevOps used by the current host (organization)
        /// </summary>
        [JsonPropertyName("serviceVersion")]
        public string? ServiceVersion { get; }

        /// <summary>
        /// DevOps host level
        /// </summary>
        [JsonPropertyName("type")]
        public HostType? HostType { get; }
    }

    public class UserDetails
    {
        /// <summary>
        /// Identity descriptor used to represent this user. In the format of {subject-type}.{base64-encoded-subject-id}
        /// </summary>
        [JsonPropertyName("descriptor")]
        public string? Descriptor { get; }

        /// <summary>
        /// Unique id for the user
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; }

        /// <summary>
        /// Name of the user (email/login)
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; }

        /// <summary>
        /// The user's display name (First name / Last name)
        /// </summary>
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; }

        /// <summary>
        /// Url to the user's profile image
        /// </summary>
        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; }
    }
}