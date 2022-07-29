namespace Mittons.Azure.Devops.Extension.Service;

[AttributeUsage(System.AttributeTargets.Method)]
public class ProxyFunctionAttribute : Attribute
{
    public string FunctionName { get; }

    public ProxyFunctionAttribute(string functionName)
    {
        FunctionName = functionName;
    }
}
