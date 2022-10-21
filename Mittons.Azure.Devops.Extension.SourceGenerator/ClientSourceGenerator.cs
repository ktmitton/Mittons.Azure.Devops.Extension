using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

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

        private string ReadResource(string path)
        {
            var assembly = Assembly.GetAssembly(typeof(ClientSourceGenerator));

            var resourceName = $"{assembly.GetName().Name}.{Regex.Replace(path, @"[\\/]", ".")}";

            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(resourceStream))
            {
                return reader.ReadToEnd();
            }
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // Get our SyntaxReceiver back
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
            {
                throw new ArgumentException("Received invalid receiver in Execute step");
            }

            var extensionsPartial = ReadResource(@"Client\Extensions.mustache");
            var implementationPartial = ReadResource(@"Client\Implementation.mustache");
            var template = ReadResource(@"Client\Template.mustache");

            Handlebars.RegisterTemplate("Extensions", extensionsPartial);
            Handlebars.RegisterTemplate("Implementation", implementationPartial);
            var compiled = Handlebars.Compile(template);

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

                var data = new
                {
                    Namespace = classNamespace,
                    ClassName = className,
                    InterfaceName = interfaceName,
                    ResourceAreaId = resourceAreaId,
                    Methods = methods.Select(method => {
                        var clientRequestAttribute = method.AttributeLists
                            .Select(x => x.Attributes)
                            .SelectMany(x => x)
                            .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequest");

                        return new
                        {
                            MethodName = method.Identifier.Text,
                            ReturnType = method.ReturnType.ToString(),
                            ParametersList = string.Join(", ", method.ParameterList.Parameters.Select(x => $"{x.Type} {x.Identifier.ValueText}")),
                            RequestApiVersion = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[0].Expression).ToString(),
                            RequestAcceptType = clientRequestAttribute.ArgumentList.Arguments.Count > 3 ? serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[3].Expression).ToString() : "application/json",
                            RouteTemplate = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[2].Expression).ToString()
                        };
                    })
                };

                context.AddSource($"{className}.g.cs", SourceText.From(compiled(data), Encoding.UTF8));
            }


            //     sourceBuilder.AppendLine("using System.Collections.Generic;");
            //     sourceBuilder.AppendLine("using System.Net.Http;");
            //     sourceBuilder.AppendLine("using System.Net.Http.Headers;");
            //     sourceBuilder.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            //     sourceBuilder.AppendLine("using Mittons.Azure.Devops.Extension.Sdk;");
            //     sourceBuilder.AppendLine("using Mittons.Azure.Devops.Extension.Service;");

            //     sourceBuilder.AppendLine(1, $"internal class {className} : RestClient, {interfaceName}");
            //     sourceBuilder.AppendLine(1, "{");
            //     sourceBuilder.AppendLine(2, $"public {className}(HttpClient httpClient) : base(httpClient)");
            //     sourceBuilder.AppendLine(2, "{");
            //     sourceBuilder.AppendLine(2, "}");

            //     foreach (var method in methods)
            //     {
            //         var model = context.Compilation.GetSemanticModel(method.SyntaxTree);
            //         var methodName = method.Identifier.Text;
            //         var clientRequestAttribute = method.AttributeLists
            //             .Select(x => x.Attributes)
            //             .SelectMany(x => x)
            //             .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequest");

            //         var apiVersion = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[0].Expression).ToString();
            //         var httpMethod = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[1].Expression).ToString();
            //         var routeTemplate = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[2].Expression).ToString();
            //         var acceptType = "application/json";
            //         if (clientRequestAttribute.ArgumentList.Arguments.Count > 3)
            //         {
            //             acceptType = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[3].Expression).ToString();
            //         }

            //         sourceBuilder.AppendLine();
            //         sourceBuilder.Append(2, $"public {method.ReturnType.ToString()} {methodName}");
            //         if (method.TypeParameterList?.Parameters.Any() ?? false)
            //         {
            //             sourceBuilder.Append($"<{string.Join(", ", method.TypeParameterList.Parameters.Select(x => x.Identifier.ValueText))}>");
            //         }
            //         sourceBuilder.AppendLine($"({string.Join(", ", method.ParameterList.Parameters.Select(x => $"{x.Type} {x.Identifier.ValueText}"))})");
            //         sourceBuilder.AppendLine(2, "{");
            //         var parameters = new List<string>();

            //         foreach (var parameter in method.ParameterList.Parameters)
            //         {
            //             var queryAttribute = parameter.AttributeLists
            //                 .Select(x => x.Attributes)
            //                 .SelectMany(x => x)
            //                 .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequestQueryParameter");

            //             if (!(queryAttribute is null))
            //             {
            //                 parameters.Add(parameter.Identifier.ValueText);
            //             }
            //         }

            //         if (parameters.Any())
            //         {
            //             sourceBuilder.AppendLine(3, "var queryParameters = new Dictionary<string, object?>{");
            //             sourceBuilder.AppendLine(4, $"{string.Join(",\n\t\t\t\t", parameters.Select(x => $"{{ \"{x}\", {x} }}"))}");
            //             sourceBuilder.AppendLine(3, "};");
            //         }
            //         else
            //         {
            //             sourceBuilder.AppendLine(3, "var queryParameters = new Dictionary<string, object?>();");
            //         }
            //         sourceBuilder.AppendLine();

            //         var bodyName = default(string);
            //         var bodyType = default(string);
            //         foreach (var parameter in method.ParameterList.Parameters)
            //         {
            //             var bodyAttribute = parameter.AttributeLists
            //                 .Select(x => x.Attributes)
            //                 .SelectMany(x => x)
            //                 .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequestBody");

            //             if (!(bodyAttribute is null))
            //             {
            //                 bodyName = parameter.Identifier.ValueText;
            //                 bodyType = parameter.Type.ToString();
            //             }
            //         }
            //         if (!string.IsNullOrWhiteSpace(bodyName))
            //         {
            //             sourceBuilder.AppendLine(3, $"return base.SendRequestAsync<{bodyType}, {method.ReturnType.ToString().Replace("Task<", "").Replace(">", "")}>(");
            //             sourceBuilder.AppendLine(4, $"body: {bodyName},");
            //         }
            //         else
            //         {
            //             sourceBuilder.AppendLine(3, $"return base.SendRequestAsync<{method.ReturnType.ToString().Replace("Task<", "").Replace(">", "")}>(");
            //         }
            //         sourceBuilder.AppendLine(4, $"queryParameters: queryParameters,");
            //         sourceBuilder.AppendLine(4, $"acceptType: \"{acceptType}\",");
            //         sourceBuilder.AppendLine(4, $"apiVersion: \"{apiVersion}\",");
            //         sourceBuilder.AppendLine(4, $"method: new HttpMethod(\"{httpMethod}\"),");
            //         sourceBuilder.AppendLine(4, $"route: $\"{routeTemplate}\"");
            //         sourceBuilder.AppendLine(3, ");");
            //         sourceBuilder.AppendLine(2, "}");
            //     }

            //     sourceBuilder.AppendLine(1, "}");

            //     sourceBuilder.AppendLine("}");

            //     context.AddSource($"{className}.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            // }
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
