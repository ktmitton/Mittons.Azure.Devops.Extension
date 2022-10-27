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

        public ClientRequestAttribute(string apiVersion, string method, string routeTemplate)
            : this(apiVersion, method, routeTemplate, "application/json")
        {
        }

        public ClientRequestAttribute(string apiVersion, string method, string routeTemplate, string acceptType)
            : this(apiVersion, method, routeTemplate, acceptType, default(string))
        {
        }

        public ClientRequestAttribute(string apiVersion, string method, string routeTemplate, string acceptType, string? contentType)
        {
            ApiVersion = apiVersion;
            Method = method;
            RouteTemplate = routeTemplate;
            AcceptType = acceptType;
            ContentType = contentType;
        }
    }
}
