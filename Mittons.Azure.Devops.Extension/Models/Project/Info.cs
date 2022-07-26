using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Models.Project;

public record Info
{
    [JsonPropertyName("id")]
    public Guid? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
