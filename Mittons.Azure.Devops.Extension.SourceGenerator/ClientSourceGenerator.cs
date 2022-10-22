using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Mittons.Azure.Devops.Extension.SourceGenerator.Utilities;

namespace Mittons.Azure.Devops.Extension.SourceGenerator
{
    public static class StringBuilderExtensions
    {
        public static void AppendLine(this StringBuilder @this, int indent, string content)
            => @this.AppendLine($"{string.Join("", Enumerable.Repeat("\t", indent))}{content}");

        public static void Append(this StringBuilder @this, int indent, string content)
            => @this.Append($"{string.Join("", Enumerable.Repeat("\t", indent))}{content}");
    }

    [Generator]
    public class ClientSourceGenerator : ISourceGenerator
    {
        private class Property
        {
            public string Name { get; set; }
        }

        private class RequestBodyDefinition
        {
            public string ContentType { get; set; }

            public bool IsUnknownBody => !IsJsonBody && !IsByteArrayBody && !(ContentType is null);

            private bool _isJsonBody = false;
            public bool IsJsonBody
            {
                get => _isJsonBody;

                set
                {
                    if (value && _isByteArrayBody)
                    {
                        throw new InvalidOperationException("A byte array body has already been set");
                    }

                    _isJsonBody = value;
                }
            }

            private bool _isByteArrayBody = false;
            public bool IsByteArrayBody
            {
                get => _isByteArrayBody;

                set
                {
                    if (value && _isJsonBody)
                    {
                        throw new InvalidOperationException("The request body has already been set to a json body");
                    }

                    _isByteArrayBody = value;
                }
            }

            public List<Property> JsonProperties { get; set; } = new List<Property>();

            private string _byteArrayParameter = default(string);
            public string ByteArrayParameter
            {
                get => _byteArrayParameter;

                set
                {
                    if (_byteArrayParameter is null)
                    {
                        _byteArrayParameter = value;
                    }
                    else
                    {
                        throw new InvalidOperationException("The request byte array was already set and cannot be set again");
                    }
                }
            }
        }

        // determine the namespace the class/enum/struct is declared in, if any
        static string GetNamespace(InterfaceDeclarationSyntax syntax)
        {
            // If we don't have a namespace at all we'll return an empty string
            // This accounts for the "default namespace" case
            var nameSpace = string.Empty;

            // Get the containing syntax node for the type declaration
            // (could be a nested type, for example)
            SyntaxNode potentialNamespaceParent = syntax.Parent;

            // Keep moving "out" of nested classes etc until we get to a namespace
            // or until we run out of parents
            while (potentialNamespaceParent != null &&
                !typeof(NamespaceDeclarationSyntax).IsAssignableFrom(potentialNamespaceParent.GetType()) &&
                !typeof(FileScopedNamespaceDeclarationSyntax).IsAssignableFrom(potentialNamespaceParent.GetType()))
            {
                potentialNamespaceParent = potentialNamespaceParent.Parent;
            }

            // Build up the final namespace by looping until we no longer have a namespace declaration
            if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
            {
                // We have a namespace. Use that as the type
                nameSpace = namespaceParent.Name.ToString();

                // Keep moving "out" of the namespace declarations until we
                // run out of nested namespace declarations
                while (true)
                {
                    if (!typeof(NamespaceDeclarationSyntax).IsAssignableFrom(potentialNamespaceParent.Parent.GetType()))
                    {
                        break;
                    }

                    // Add the outer namespace as a prefix to the final namespace
                    nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                    namespaceParent = namespaceParent.Parent as NamespaceDeclarationSyntax;
                }
            }

            // return the final namespace
            return nameSpace;
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
            {
                throw new ArgumentException("Received invalid receiver in Execute step");
            }

            var compiled = Mustache.CompileTemplate("Mittons.Azure.Devops.Extension.SourceGenerator.Client", "Template");

            foreach (var ids in receiver.DecoratorRequestingInterfaces)
            {
                var classNamespace = GetNamespace(ids);
                var serviceModel = context.Compilation.GetSemanticModel(ids.SyntaxTree);

                var serviceAttribute = ids.AttributeLists
                    .Select(x => x.Attributes)
                    .SelectMany(x => x)
                    .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "GenerateClient");

                var resourceAreaId = serviceModel.GetConstantValue(serviceAttribute.ArgumentList.Arguments[0].Expression).ToString();

                var interfaceName = ids.Identifier.ValueText;
                var className = ids.Identifier.ValueText.Substring(1);
                var methods = ids.DescendantNodes().OfType<MethodDeclarationSyntax>();

                var convertedMethods = methods.Select(method =>
                {
                    var clientRequestAttribute = method.AttributeLists
                        .Select(x => x.Attributes)
                        .SelectMany(x => x)
                        .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequest");

                    var queryParameters = new List<string>();

                    var requestBody = new RequestBodyDefinition();
                    foreach (var parameter in method.ParameterList.Parameters)
                    {
                        var queryAttribute = parameter.AttributeLists
                            .Select(x => x.Attributes)
                            .SelectMany(x => x)
                            .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequestQueryParameter");

                        if (!(queryAttribute is null))
                        {
                            queryParameters.Add(parameter.Identifier.ValueText);
                        }

                        var byteArrayBodyAttribute = parameter.AttributeLists
                            .Select(x => x.Attributes)
                            .SelectMany(x => x)
                            .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientByteArrayRequestBodyAttribute");

                        if (!(byteArrayBodyAttribute is null))
                        {
                            requestBody.IsByteArrayBody = true;

                            requestBody.ByteArrayParameter = parameter.Identifier.ValueText;
                        }

                        var jsonBodyAttribute = parameter.AttributeLists
                            .Select(x => x.Attributes)
                            .SelectMany(x => x)
                            .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientJsonRequestBodyParameterAttribute");

                        if (!(jsonBodyAttribute is null))
                        {
                            requestBody.IsJsonBody = true;

                            requestBody.JsonProperties.Add(new Property { Name = parameter.Identifier.ValueText });
                        }
                    }

                    if (clientRequestAttribute.ArgumentList.Arguments.Count() > 4)
                    {
                        requestBody.ContentType = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[4].Expression).ToString();
                    }
                    else if (requestBody.IsJsonBody)
                    {
                        requestBody.ContentType = "application/json";
                    }
                    else if (requestBody.IsByteArrayBody)
                    {
                        requestBody.ContentType = MediaTypeNames.Application.Octet;
                    }

                    return new
                    {
                        MethodName = method.Identifier.Text,
                        HttpMethod = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[1].Expression).ToString(),
                        ReturnType = method.ReturnType.ToString(),
                        InnerReturnType = method.ReturnType.ToString().Replace("Task<", "").Replace(">", ""),
                        ParametersList = string.Join(", ", method.ParameterList.Parameters.Select(x => $"{x.Type} {x.Identifier.ValueText}")),
                        RequestApiVersion = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[0].Expression).ToString(),
                        RequestAcceptType = clientRequestAttribute.ArgumentList.Arguments.Count > 3 ? serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[3].Expression).ToString() : "application/json",
                        RouteTemplate = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[2].Expression).ToString(),
                        QueryParametersList = queryParameters.Select(x => new { Name = x }),
                        RequestBody = requestBody
                    };
                });

                var data = new
                {
                    Namespace = classNamespace,
                    ClassName = className,
                    InterfaceName = interfaceName,
                    ResourceAreaId = resourceAreaId,
                    ByteArrayMethods = convertedMethods.Where(x => x.InnerReturnType == "byte[]"),
                    JsonMethods = convertedMethods.Where(x => x.RequestAcceptType == "application/json"),
                    StringMethods = convertedMethods.Where(x => x.RequestAcceptType != "application/json" && x.RequestAcceptType != MediaTypeNames.Application.Zip && x.InnerReturnType == "string"),
                    XmlDocumentMethods = convertedMethods.Where(x => x.InnerReturnType == "XmlDocument"),
                    XmlMethods = convertedMethods.Where(x => x.RequestAcceptType == "application/xml" && x.InnerReturnType != "string" && x.InnerReturnType != "byte[]" && x.InnerReturnType != "XmlDocument"),
                    ZipArchiveMethods = convertedMethods.Where(x => x.RequestAcceptType == MediaTypeNames.Application.Zip && x.InnerReturnType == "ZipArchive")
                };

                context.AddSource($"{className}.g.cs", SourceText.From(compiled(data), Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        class SyntaxReceiver : ISyntaxReceiver
        {
            public List<InterfaceDeclarationSyntax> DecoratorRequestingInterfaces { get; } = new List<InterfaceDeclarationSyntax>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (!(syntaxNode is InterfaceDeclarationSyntax ids) || !ids.AttributeLists.Any())
                {
                    return;
                }

                var requiresGeneration = ids.AttributeLists
                    .Select(x => x.Attributes)
                    .SelectMany(x => x)
                    .Select(x => x.Name)
                    .OfType<IdentifierNameSyntax>()
                    .Any(x => x.Identifier.ValueText == "GenerateClient");

                if (requiresGeneration)
                {
                    DecoratorRequestingInterfaces.Add(ids);
                }
            }
        }
    }
}
