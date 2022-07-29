using Mittons.Azure.Devops.Extension.Attributes;

namespace Mittons.Azure.Devops.Extension.Client;

public record GitAnnotatedTag(string message, string name, string objectId, string taggedBy, string taggedObject, string url);

public record GitBlobReference(object _links, string objectId, int size, string url);

[GenerateClient("4e080c62-fa21-4fbc-8fef-2a10a2b38049")]
public interface IGitClient
{
    [ClientRequest("5.2-preview.1", "POST", "{projectId}/_apis/git/repositories/{repositoryId}/annotatedTags/")]
    Task<GitAnnotatedTag> CreateAnnotatedTagAsync([ClientRequestBody] GitAnnotatedTag tagObject, Guid projectId, Guid repositoryId);

    [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/repositories/{repositoryId}/annotatedTags/{objectId}")]
    Task<GitAnnotatedTag> GetAnnotatedTagAsync(Guid projectId, Guid repositoryId, string objectId);

    [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs/{sha1}")]
    Task<GitBlobReference> GetBlob(Guid projectId, Guid repositoryId, string sha1, [ClientRequestQueryParameter] bool? download, [ClientRequestQueryParameter] string? fileName, [ClientRequestQueryParameter] bool? resolveLfs);

    [ClientRequest("5.2-preview.1", "GET", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs/{sha1}", "application/octet-stream")]
    Task<byte[]> GetBlobContent(Guid projectId, Guid repositoryId, string sha1, [ClientRequestQueryParameter] bool? download, [ClientRequestQueryParameter] string? fileName, [ClientRequestQueryParameter] bool? resolveLfs);

    [ClientRequest("5.2-preview.1", "POST", "{projectId}/_apis/git/repositories/{repositoryId}/Blobs", "application/zip")]
    Task<byte[]> GetBlobZip(Guid projectId, Guid repositoryId, [ClientRequestQueryParameter] string? filename, [ClientRequestBody] string[] blobIds);
}
