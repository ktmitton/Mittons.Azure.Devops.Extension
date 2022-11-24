using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Mittons.Azure.Devops.Extension.SourceGenerator.Extensions;
using Mittons.Azure.Devops.Extension.SourceGenerator.Utilities;

namespace Mittons.Azure.Devops.Extension.SourceGenerator.Service
{
    [Generator]
    public class ServiceSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
            {
                throw new ArgumentException("Received invalid receiver in Execute step");
            }

            var compiledModel = Mustache.CompileEnvironment<ServiceSourceGenerator>("Mittons.Azure.Devops.Extension.SourceGenerator.Service", "RemoteProxyFunctionDefinition");
            context.AddSource($"RemoteProxyFunctionDefinition.g.cs", SourceText.From(compiledModel(new { }), Encoding.UTF8));

            var compiled = Mustache.CompileEnvironment<ServiceSourceGenerator>("Mittons.Azure.Devops.Extension.SourceGenerator.Service", "Template");

            foreach (var ids in receiver.DecoratorRequestingInterfaces)
            {
                var classNamespace = ids.GetNamespace();
                var serviceModel = context.Compilation.GetSemanticModel(ids.SyntaxTree);

                var serviceAttribute = ids.AttributeLists
                    .Select(x => x.Attributes)
                    .SelectMany(x => x)
                    .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "GenerateService");

                var contributionId = serviceModel.GetConstantValue(serviceAttribute.ArgumentList.Arguments[0].Expression).ToString();

                var interfaceName = ids.Identifier.ValueText;
                var className = ids.Identifier.ValueText.Substring(1);

                var methods = ids.DescendantNodes().OfType<MethodDeclarationSyntax>();

                var remoteProxyFunctionDefinitions = methods.Select(method =>
                {
                    var remoteProxyFunctionAttribute = method.AttributeLists
                        .Select(x => x.Attributes)
                        .SelectMany(x => x)
                        .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "RemoteProxyFunction");

                    var jsonPropertyName = serviceModel.GetConstantValue(remoteProxyFunctionAttribute.ArgumentList.Arguments[0].Expression).ToString();
                    var methodName = method.Identifier.Text;

                    return $"[property: JsonPropertyName(\"{jsonPropertyName}\")] RemoteProxyFunctionDefinition {methodName}";
                });

                var remoteProxyFunctionImplementations = methods.Select(method =>
                {
                    var remoteProxyFunctionAttribute = method.AttributeLists
                        .Select(x => x.Attributes)
                        .SelectMany(x => x)
                        .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "RemoteProxyFunction");

                    var methodName = method.Identifier.Text;

                    var innerReturnType = Regex.IsMatch(method.ReturnType.ToString(), @"^Task<.*>$") ? Regex.Replace(method.ReturnType.ToString(), @"^Task<(.*)>$", "$1") : string.Empty;

                    var proxyParameters = new string[]
                    {
                        method.ParameterList.Parameters.First(x => x.Type.ToString() == "CancellationToken").Identifier.ValueText.ToString()
                    };

                    proxyParameters = proxyParameters.Concat(method.ParameterList.Parameters.Where(x => x.Type.ToString() != "CancellationToken").Select(x => x.Identifier.ValueText)).ToArray();

                    return new
                    {
                        MethodName = methodName,
                        ArgumentParameters = string.Join(", ", method.ParameterList.Parameters.Select(x => $"{x.Type} {x.Identifier.ValueText} {x.Default}".Trim())),
                        ProxyParameters = string.Join(", ", proxyParameters),
                        InnerReturnType = innerReturnType
                    };
                });

                var data = new
                {
                    Namespace = classNamespace,
                    ClassName = className,
                    InterfaceName = interfaceName,
                    ContributionId = contributionId,
                    RemoteProxyFunctionDefinitions = string.Join(", ", remoteProxyFunctionDefinitions),
                    RemoteProxyFunctionVoidImplementations = remoteProxyFunctionImplementations.Where(x => x.InnerReturnType.Length == 0),
                    RemoteProxyFunctionGenericImplementations = remoteProxyFunctionImplementations.Where(x => x.InnerReturnType.Length > 0)
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
                    .Any(x => x.Identifier.ValueText == "GenerateService");

                if (requiresGeneration)
                {
                    DecoratorRequestingInterfaces.Add(ids);
                }
            }
        }
    }
}
