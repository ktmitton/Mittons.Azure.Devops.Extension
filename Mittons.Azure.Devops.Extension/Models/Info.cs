using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Models;

public record ProjectInfo
{
    [JsonPropertyName("id")]
    public Guid? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
