using System.Text.Json.Serialization;

namespace Mittons.Azure.Devops.Extension.Service.GlobalMessages;

public class Link
{
    /**
     * Hyperlink text
     */
    [JsonPropertyName("name")]
    public string Name { get; }

    /**
     * Url of the link target
     */
    [JsonPropertyName("href")]
    public string Href { get; }

    public Link(string name, string href)
    {
        Name = name;
        Href = href;
    }
}