using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Sdk.Service.ProjectPage;

public record ProjectInfo
{
    [JsonPropertyName("id")]
    public Guid? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
