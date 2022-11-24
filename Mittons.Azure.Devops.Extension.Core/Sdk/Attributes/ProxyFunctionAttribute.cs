using System;

namespace Mittons.Azure.Devops.Extension.Sdk.Attributes
{
    [AttributeUsage(System.AttributeTargets.Method)]
    public class RemoteProxyFunctionAttribute : Attribute
    {
        public string FunctionName { get; }

        public RemoteProxyFunctionAttribute(string functionName)
        {
            FunctionName = functionName;
        }
    }
}
