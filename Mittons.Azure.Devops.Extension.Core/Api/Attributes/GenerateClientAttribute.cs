using System;
using System.Collections.Generic;
using System.Linq;

namespace Mittons.Azure.Devops.Extension.Api.Attributes
{
    [AttributeUsage(System.AttributeTargets.Interface)]
    public class GenerateClientAttribute : Attribute
    {
        public string ResourceAreaId { get; }

        public IEnumerable<VsoScope> Scopes { get; }

        public GenerateClientAttribute(string resourceAreaId, params VsoScope[] scopes)
        {
            ResourceAreaId = resourceAreaId;
            Scopes = scopes;
        }
    }
}
