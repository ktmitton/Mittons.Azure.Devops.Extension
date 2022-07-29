using Mittons.Azure.Devops.Extension.Attributes;

namespace Mittons.Azure.Devops.Extension.Client;

public record GitAnnotatedTag(string message, string name, string objectId, string taggedBy, string taggedObject, string url);

[GenerateClient("4e080c62-fa21-4fbc-8fef-2a10a2b38049")]
public interface IGitClient
{
    [ClientRequest("5.2-preview.1", "POST", "{projectId}/_apis/git/repositories/{repositoryId}/annotatedTags/")]
    Task<GitAnnotatedTag> CreateAnnotatedTagAsync([ClientRequestBody] GitAnnotatedTag tagObject, Guid projectId, Guid repositoryId);
}
