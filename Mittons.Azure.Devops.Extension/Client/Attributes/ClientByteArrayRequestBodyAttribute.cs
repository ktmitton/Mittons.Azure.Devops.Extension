using System.Net.Mime;

namespace Mittons.Azure.Devops.Extension.Attributes;

[AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false)]
public class ClientByteArrayRequestBodyAttribute : Attribute
{
    public string ContentType { get; set; }

    public ClientByteArrayRequestBodyAttribute() : this(MediaTypeNames.Application.Octet) { }

    public ClientByteArrayRequestBodyAttribute(string contentType)
    {
        ContentType = contentType;
    }
}
