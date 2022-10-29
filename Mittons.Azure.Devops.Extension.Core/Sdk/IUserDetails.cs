namespace Mittons.Azure.Devops.Extension.Sdk
{
    public interface IUserDetails
    {
        /// <summary>
        /// Identity descriptor used to represent this user. In the format of {subject-type}.{base64-encoded-subject-id}
        /// </summary>
        string? Descriptor { get; }

        /// <summary>
        /// Unique id for the user
        /// </summary>
        string? Id { get; }

        /// <summary>
        /// Name of the user (email/login)
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// The user's display name (First name / Last name)
        /// </summary>
        string? DisplayName { get; }

        /// <summary>
        /// Url to the user's profile image
        /// </summary>
        string? ImageUrl { get; }
    }
}
