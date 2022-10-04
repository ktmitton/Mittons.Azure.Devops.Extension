namespace Mittons.Azure.Devops.Extension.Service;

[AttributeUsage(System.AttributeTargets.Interface)]
public class GenerateServiceAttribute : Attribute
{
    public string ContributionId { get; }

    public GenerateServiceAttribute(string contributionId)
    {
        ContributionId = contributionId;
    }
}
