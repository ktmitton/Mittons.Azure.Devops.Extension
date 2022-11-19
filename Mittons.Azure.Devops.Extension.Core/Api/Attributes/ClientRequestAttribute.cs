using System;

namespace Mittons.Azure.Devops.Extension.Api.Attributes
{
    [AttributeUsage(System.AttributeTargets.Method)]
    public class ClientRequestAttribute : Attribute
    {
        public string ApiVersion { get; set; }

        public string Method { get; set; }

        public string RouteTemplate { get; set; }

        public string AcceptType { get; set; }

        public string? ContentType { get; set; }

        public ClientRequestAttribute(string apiVersion, string method, string routeTemplate, string acceptType = "application/json", string? contentType = default(string))
        {
            ApiVersion = apiVersion;
            Method = method;
            RouteTemplate = routeTemplate;
            AcceptType = acceptType;
            ContentType = contentType;
        }
    }
}
