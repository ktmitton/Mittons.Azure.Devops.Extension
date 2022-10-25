using System.IO.Compression;
using System.Net.Mime;
using System.Xml;
using Mittons.Azure.Devops.Extension.Attributes;
using Mittons.Azure.Devops.Extension.Client;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

[GenerateClient(ResourceAreaId.Git)]
public interface ITestGitClient
{
    [ClientRequest("5.2-preview.1", "GET", "/get")]
    Task<string> GetWithApiVersion1();

    [ClientRequest("7.3", "GET", "/get")]
    Task<string> GetWithApiVersion2();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Json)]
    Task<string> GetWithExplicitJsonMediaType();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Plain)]
    Task<string> GetWithExplicitPlainTextMediaType();

    [ClientRequest("5.2-preview.1", "GET", "/get")]
    Task<string> GetWithPath1();

    [ClientRequest("5.2-preview.1", "GET", "/path")]
    Task<string> GetWithPath2();

    [ClientRequest("5.2-preview.1", "GET", "/get/{routeParam1}/{routeParam2}")]
    Task<string> GetWithRouteParameters1(int routeParam1, string routeParam2);

    [ClientRequest("5.2-preview.1", "GET", "/{firstRouteParam}/get/{secondRouteParam}")]
    Task<string> GetWithRouteParameters2(Guid firstRouteParam, decimal secondRouteParam);

    [ClientRequest("5.2-preview.1", "GET", "/get")]
    Task<string> GetWithQueryParameters1([ClientRequestQueryParameter] string someParameter);

    [ClientRequest("5.2-preview.1", "GET", "/get")]
    Task<string> GetWithQueryParameters2([ClientRequestQueryParameter] bool? testParameter2, [ClientRequestQueryParameter] string? otherParameter, [ClientRequestQueryParameter] decimal? testParameter1);

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Plain)]
    Task<string> PlainTextStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Plain)]
    Task<string?> PlainTextNuallableStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Plain)]
    Task<byte[]> PlainTextByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Json)]
    Task<NameTestModel> JsonNameModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Json)]
    Task<AddressTestModel> JsonAddressModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Json)]
    Task<NameTestModel?> JsonNullableNameModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Json)]
    Task<AddressTestModel?> JsonNullableAddressModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Zip)]
    Task<byte[]> ZipByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Zip)]
    Task<ZipArchive> ZipArchiveResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Octet)]
    Task<byte[]> OctetResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Html)]
    Task<byte[]> HtmlByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Text.Html)]
    Task<string> HtmlStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/svg+xml")]
    Task<byte[]> SvgByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/svg+xml")]
    Task<string> SvgStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/svg+xml")]
    Task<XmlDocument> SvgXmlDocumentResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/xaml+xml")]
    Task<byte[]> XamlByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/xaml+xml")]
    Task<string> XamlStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", "image/xaml+xml")]
    Task<XmlDocument> XamlXmlDocumentResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<byte[]> XmlByteArrayResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<string> XmlStringResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<XmlDocument> XmlDocumentResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<NameTestModel> XmlNameModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<AddressTestModel> XmlAddressModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<NameTestModel?> XmlNullableNameModelResponse();

    [ClientRequest("5.2-preview.1", "GET", "/get", MediaTypeNames.Application.Xml)]
    Task<AddressTestModel?> XmlNullableAddressModelResponse();

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json)]
    Task<string> PostEmptyBody();

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json)]
    Task<string> PostByteArrayBody([ClientByteArrayRequestBodyAttribute] byte[] requestBody);

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json, MediaTypeNames.Application.Octet)]
    Task<string> PostByteArrayOctectBody([ClientByteArrayRequestBodyAttribute] byte[] requestBody);

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json, MediaTypeNames.Text.Plain)]
    Task<string> PostByteArrayPlainTextBody([ClientByteArrayRequestBodyAttribute] byte[] requestBody);

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json, MediaTypeNames.Application.Json)]
    Task<string> PostEmptyJsonBody();

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json, MediaTypeNames.Application.Json)]
    Task<string> PostNameJsonBody([ClientJsonRequestBodyParameterAttribute] string firstName, [ClientJsonRequestBodyParameterAttribute] string lastName);

    [ClientRequest("5.2-preview.1", "POST", "/post", MediaTypeNames.Application.Json, "application/json+patch")]
    Task<string> PostNameJsonPatchBody([ClientJsonRequestBodyParameterAttribute] string firstName, [ClientJsonRequestBodyParameterAttribute] string lastName);
}
