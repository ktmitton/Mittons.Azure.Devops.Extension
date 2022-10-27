using System;

namespace Mittons.Azure.Devops.Extension.Api.Attributes
{
    [AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false)]
    public class ClientJsonRequestBodyParameterAttribute : Attribute { }
}
