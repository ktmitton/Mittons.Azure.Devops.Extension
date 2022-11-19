using System;
using System.Net.Mime;

namespace Mittons.Azure.Devops.Extension.Api.Attributes
{
    [AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false)]
    public class ClientByteArrayRequestBodyAttribute : Attribute
    {
        public string ContentType { get; set; }

        public ClientByteArrayRequestBodyAttribute(string contentType = MediaTypeNames.Application.Octet)
        {
            ContentType = contentType;
        }
    }
}
