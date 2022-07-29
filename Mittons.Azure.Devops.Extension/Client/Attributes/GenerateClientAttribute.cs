namespace Mittons.Azure.Devops.Extension.Attributes;

[AttributeUsage(System.AttributeTargets.Interface)]
public class GenerateClientAttribute : Attribute
{
    public string ResourceAreaId { get; }

    public GenerateClientAttribute(string resourceAreaId)
    {
        ResourceAreaId = resourceAreaId;
    }
}
