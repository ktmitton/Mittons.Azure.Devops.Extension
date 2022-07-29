using Mittons.Azure.Devops.Extension.Attributes;

using ChangeCountDictionary = System.Collections.Generic.Dictionary<Mittons.Azure.Devops.Extension.Client.VersionControlChangeType, int>;

using Links = System.Collections.Generic.Dictionary<string, Mittons.Azure.Devops.Extension.Client.Link>;

namespace Mittons.Azure.Devops.Extension.Client;

public record Link(Uri href);

public record GitAnnotatedTag(string message, string name, string objectId, string taggedBy, string taggedObject, Uri url);

public record GitBlobReference(Links _links, string objectId, int size, Uri url);

public enum GitVersionOptions
{
    None = 0,
    PreviousChange = 1,
    FirstParent = 2
}

public enum GitVersionType
{
    Branch = 0,
    Tag = 1,
    Commit = 2
}

public enum VersionControlChangeType
{
    None = 0,
    Add = 1,
    Edit = 2,
    Encoding = 4,
    Rename = 8,
    Delete = 16,
    Undelete = 32,
    Branch = 64,
    Merge = 128,
    Lock = 256,
    Rollback = 512,
    SourceRename = 1024,
    TargetRename = 2048,
    Property = 4096,
    All = 8191
}

public enum ItemContentType
{
    RawText = 0,
    Base64Encoded = 1
}

public enum GitObjectType
{
    Bad = 0,
    Commit = 1,
    Tree = 2,
    Blob = 3,
    Tag = 4,
    Ext2 = 5,
    OfsDelta = 6,
    RefDelta = 7
}

public record GitVersionDescriptor(string version, GitVersionOptions versionOptions, GitVersionType versionType);

public record GitBranchStats(int aheadCount, int behindCount, GitCommitReference commit, bool isBaseVersion, string name);

public record GitCommitReference(Links _links, GitUserData author, ChangeCountDictionary changeCounts, GitChange[] changes, string comment, bool commentTruncated, string commitId, GitUserData committer, string[] parents, GitPushReference push, Uri remoteUrl, GitStatus[] statuses, Uri url, ResourceReference[] workItems);

public record GitUserData(DateTime date, string email, Uri imageUrl, string name);

public abstract record Change<T>(VersionControlChangeType ChangeType, T item, ItemContent newContent, string sourceServerItem, Uri url);

public record ItemContent(string content, ItemContentType contentType);

public record GitChange(VersionControlChangeType ChangeType, GitItem item, ItemContent newContent, string sourceServerItem, Uri url, int changeId, GitTemplate newContentTemplate, string originalPath)
    : Change<GitItem>(ChangeType, item, newContent, sourceServerItem, url);

public record GitTemplate(string name, string type);

public record GitItem(string commitId, GitObjectType gitObjectType, GitCommitReference latestProcessedChange, string objectId, string originalObjectId);

public record GitPushReference(Links _links, DateTime date, string pushCorrelationId, IdentityReference pushedBy, int pushId, Uri url);

public record IdentityReference(Links _links, string descriptor, string displayName, Uri url, string directoryAlias, string id, Uri imageUrl, bool inactive, bool isAadIdentity, bool isContainer, bool isDeletedInOrigin, string profileUrl, string uniqueName)
    : GraphSubjectBase(_links, descriptor, displayName, url);

public record GraphSubjectBase(Links _links, string descriptor, string displayName, Uri url);

public record GitStatus(Links _links, GitStatusContext context, IdentityReference createdBy, DateTime creationDate, string description, int id, GitStatusState state, Uri targetUrl, DateTime updatedDate);

public record GitStatusContext(string genre, string name);

public enum GitStatusState
{
    NotSet = 0,
    Pending = 1,
    Succeeded = 2,
    Failed = 3,
    Error = 4,
    NotApplicable = 5
}

public record ResourceReference(string id, Uri url);

public record GitRepository(Links _links, string defaultBranch, string id, bool isFork, string name, GitRepositoryReference parentRepository, TeamProjectReference project, Uri remoteUrl, int size, Uri sshUrl, Uri url, Uri[] validRemoteUrls, Uri webUrl);

public record GitRepositoryReference(TeamProjectCollectionReference collection, string id, bool isFork, string name, TeamProjectReference project, Uri remoteUrl, Uri sshUrl, Uri url);

public record TeamProjectCollectionReference(string id, string name, string url);

public record TeamProjectReference(string abbreviation, Uri defaultTeamImageUrl, string description, string id, DateTime lastUpdateTime, string name, int revision, object state, Uri url, ProjectVisibility visibility);

public enum ProjectVisibility
{
    Unchanged = -1,
    Private = 0,
    Organization = 1,
    Public = 2,
    SystemPrivate = 3
}

[GenerateClient("4e080c62-fa21-4fbc-8fef-2a10a2b38049")]
public interface IGitClient
{
    [ClientRequest("5.2-preview.1", "POST", "{projectId}/_apis/git/repositories/{repositoryId}/annotatedTags/")]
    Task<GitAnnotatedTag> CreateAnnotatedTagAsync([ClientRequestBody] GitAnnotatedTag tagObject, Guid projectId, Guid repositoryId);

    [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/repositories/{repositoryId}/annotatedTags/{objectId}")]
    Task<GitAnnotatedTag> GetAnnotatedTagAsync(Guid projectId, Guid repositoryId, string objectId);

    [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs/{sha1}")]
    Task<GitBlobReference> GetBlobAsync(Guid projectId, Guid repositoryId, string sha1, [ClientRequestQueryParameter] bool? download, [ClientRequestQueryParameter] string? fileName, [ClientRequestQueryParameter] bool? resolveLfs);

    [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs/{sha1}", "application/octet-stream")]
    Task<byte[]> GetBlobContentAsync(Guid projectId, Guid repositoryId, string sha1, [ClientRequestQueryParameter] bool? download, [ClientRequestQueryParameter] string? fileName, [ClientRequestQueryParameter] bool? resolveLfs);

    [ClientRequest("5.2-preview.1", "POST", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs", "application/zip")]
    Task<byte[]> GetBlobsZipAsync(Guid projectId, Guid repositoryId, [ClientRequestQueryParameter] string? filename, [ClientRequestBody] string[] blobIds);

    [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs/{sha1}", "application/zip")]
    Task<byte[]> GetBlobZipAsync(Guid projectId, Guid repositoryId, string sha1, [ClientRequestQueryParameter] bool? download, [ClientRequestQueryParameter] string? fileName, [ClientRequestQueryParameter] bool? resolveLfs);

    [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/repositories/{repositoryId}/stats/branches")]
    Task<GitBranchStats> GetBranchAsync(Guid projectId, Guid repositoryId, [ClientRequestQueryParameter] string name, [ClientRequestQueryParameter] GitVersionDescriptor? baseVersionDescriptor);

    [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/Repositories/")]
    Task<GitRepository> GetRepositoriesAsync(Guid projectId, [ClientRequestQueryParameter] bool? includeLinks, [ClientRequestQueryParameter] bool? includeAllUrls, [ClientRequestQueryParameter] bool? includeHidden);
}
