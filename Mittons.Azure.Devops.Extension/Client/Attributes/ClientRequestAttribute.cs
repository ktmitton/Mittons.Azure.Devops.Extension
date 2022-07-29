namespace Mittons.Azure.Devops.Extension.Attributes;

[AttributeUsage(System.AttributeTargets.Method)]
public class ClientRequestAttribute : Attribute
{
    public string ApiVersion { get; set; }

    public string Method { get; set; }

    public string RouteTemplate { get; set; }

    public ClientRequestAttribute(string apiVersion, string method, string routeTemplate)
    {
        ApiVersion = apiVersion;
        Method = method;
        RouteTemplate = routeTemplate;
    }
}
