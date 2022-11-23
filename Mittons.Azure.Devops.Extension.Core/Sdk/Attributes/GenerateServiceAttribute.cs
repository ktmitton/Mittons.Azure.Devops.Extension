using System;

namespace Mittons.Azure.Devops.Extension.Sdk.Attributes
{
    [AttributeUsage(System.AttributeTargets.Interface)]
    public class GenerateServiceAttribute : Attribute
    {
        public string ContributionId { get; }

        public GenerateServiceAttribute(string contributionId)
        {
            ContributionId = contributionId;
        }
    }
}
