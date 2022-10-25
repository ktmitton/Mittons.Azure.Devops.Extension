using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Mittons.Azure.Devops.Extension.SourceGenerator.Extensions;
using Mittons.Azure.Devops.Extension.SourceGenerator.Models;
using Mittons.Azure.Devops.Extension.SourceGenerator.Utilities;

namespace Mittons.Azure.Devops.Extension.SourceGenerator.Client
{
    [Generator]
    public class ClientSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
            {
                throw new ArgumentException("Received invalid receiver in Execute step");
            }

            var compiled = Mustache.CompileTemplate("Mittons.Azure.Devops.Extension.SourceGenerator.Client", "Template");

            foreach (var ids in receiver.DecoratorRequestingInterfaces)
            {
                var classNamespace = ids.GetNamespace();
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

                    var query = new Query(method);

                    var requestBody = new RequestBody(serviceModel, method);

                    var innerReturnType = Regex.Replace(method.ReturnType.ToString(), @"^Task<(.*)>$", "$1");
                    var nonNullableInnerReturnType = Regex.Replace(innerReturnType, @"^(.*)\?$", "$1");

                    return new
                    {
                        MethodName = method.Identifier.Text,
                        HttpMethod = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[1].Expression).ToString(),
                        ReturnType = method.ReturnType.ToString(),
                        InnerReturnType = innerReturnType,
                        NonNullableInnerReturnType = nonNullableInnerReturnType,
                        IsInnerReturnTypeNullable = Regex.IsMatch(innerReturnType, @"\?$"),
                        ParametersList = string.Join(", ", method.ParameterList.Parameters.Select(x => $"{x.Type} {x.Identifier.ValueText}")),
                        RequestApiVersion = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[0].Expression).ToString(),
                        RequestAcceptType = clientRequestAttribute.ArgumentList.Arguments.Count > 3 ? serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[3].Expression).ToString() : "application/json",
                        RouteTemplate = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[2].Expression).ToString(),
                        QueryParametersList = query.Parameters,
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
                    StringMethods = convertedMethods.Where(x => x.RequestAcceptType != "application/json" && x.RequestAcceptType != MediaTypeNames.Application.Zip && Regex.IsMatch(x.InnerReturnType, @"^string\??$")),
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
