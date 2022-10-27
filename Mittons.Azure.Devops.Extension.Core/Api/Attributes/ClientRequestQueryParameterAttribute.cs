using System;

namespace Mittons.Azure.Devops.Extension.Api.Attributes
{

    [AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = true)]
    public class ClientRequestQueryParameterAttribute : Attribute { }
}
