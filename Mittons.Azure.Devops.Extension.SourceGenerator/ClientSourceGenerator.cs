using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void Execute(GeneratorExecutionContext context)
        {
            // Get our SyntaxReceiver back
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
            {
                throw new ArgumentException("Received invalid receiver in Execute step");
            }

            foreach (var ids in receiver.DecoratorRequestingInterfaces)
            {
                var serviceModel = context.Compilation.GetSemanticModel(ids.SyntaxTree);

                var serviceAttribute = ids.AttributeLists
                    .Select(x => x.Attributes)
                    .SelectMany(x => x)
                    .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "GenerateClient");

                var methods = ids.DescendantNodes().OfType<MethodDeclarationSyntax>();
                var sourceBuilder = new StringBuilder();
                var interfaceName = ids.Identifier.ValueText;
                var className = ids.Identifier.ValueText.Substring(1);

                sourceBuilder.AppendLine("using System.Collections.Generic;");
                sourceBuilder.AppendLine("using Microsoft.Extensions.DependencyInjection;");
                sourceBuilder.AppendLine("using Mittons.Azure.Devops.Extension.Sdk;");
                sourceBuilder.AppendLine("using Mittons.Azure.Devops.Extension.Service;");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine("#nullable enable");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine("namespace Mittons.Azure.Devops.Extension.Client");
                sourceBuilder.AppendLine("{");



                sourceBuilder.AppendLine($"\tinternal static class {className}Extensions");
                sourceBuilder.AppendLine(1, "{");
                sourceBuilder.AppendLine(2, $"public static IServiceCollection Add{className}(this IServiceCollection @serviceCollection)");
                sourceBuilder.AppendLine(3, $"=> @serviceCollection.AddSingleton<{ids.Identifier.ValueText}, {className}>();");
                sourceBuilder.AppendLine(1, "}");
                sourceBuilder.AppendLine();

                sourceBuilder.AppendLine(1, $"internal class {className} : RestClient, {interfaceName}");
                sourceBuilder.AppendLine(1, "{");
                sourceBuilder.AppendLine(2, $"public {className}(ISdk sdk, ILocationService locationService) : base(sdk, locationService)");
                sourceBuilder.AppendLine(2, "{");
                sourceBuilder.AppendLine(2, "}");

                foreach (var method in methods)
                {
                    var model = context.Compilation.GetSemanticModel(method.SyntaxTree);
                    var methodName = method.Identifier.Text;
                    var clientRequestAttribute = method.AttributeLists
                        .Select(x => x.Attributes)
                        .SelectMany(x => x)
                        .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequest");

                    var apiVersion = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[0].Expression).ToString();
                    var httpMethod = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[1].Expression).ToString();
                    var routeTemplate = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[2].Expression).ToString();

                    sourceBuilder.AppendLine();
                    sourceBuilder.Append(2, $"public {method.ReturnType.ToString()} {methodName}");
                    if (method.TypeParameterList?.Parameters.Any() ?? false)
                    {
                        sourceBuilder.Append($"<{string.Join(", ", method.TypeParameterList.Parameters.Select(x => x.Identifier.ValueText))}>");
                    }
                    sourceBuilder.AppendLine($"({string.Join(", ", method.ParameterList.Parameters.Select(x => $"{x.Type} {x.Identifier.ValueText}"))})");
                    sourceBuilder.AppendLine(2, "{");
                    var parameters = new List<string>();

                    foreach (var parameter in method.ParameterList.Parameters)
                    {
                        var queryAttribute = parameter.AttributeLists
                            .Select(x => x.Attributes)
                            .SelectMany(x => x)
                            .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequestQueryParameter");

                        if (!(queryAttribute is null))
                        {
                            parameters.Add(parameter.Identifier.ValueText);
                        }
                    }

                    if (parameters.Any())
                    {
                        sourceBuilder.AppendLine(3, "var queryParameters = new Dictionary<string, string>{");
                        sourceBuilder.AppendLine(4, $"{string.Join(",\n\t\t\t\t", parameters.Select(x => $"{{ \"{x}\", {x}?.ToString() ?? string.Empty }}"))}");
                        sourceBuilder.AppendLine(3, "};");
                    }
                    else
                    {
                        sourceBuilder.AppendLine(3, "var queryParameters = new Dictionary<string, string>();");
                    }

                    // sourceBuilder.AppendLine(3, "var queryParameters = new Dictionary<string, string>");
                    // sourceBuilder.AppendLine(3, "{");
                    // foreach (var parameter in method.ParameterList.Parameters)
                    // {
                    //     var queryAttribute = parameter.AttributeLists
                    //         .Select(x => x.Attributes)
                    //         .SelectMany(x => x)
                    //         .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequestQueryParameter");

                    //     if (!(queryAttribute is null))
                    //     {
                    //         sourceBuilder.AppendLine(4, $"{{ \"{parameter.Identifier.ValueText}\", {parameter.Identifier.ValueText}?.ToString() ?? string.Empty }},");
                    //     }
                    // }
                    // sourceBuilder.AppendLine(3, "}.Where(x => !string.IsNullOrWhiteSpace(x.Value));");
                    sourceBuilder.AppendLine();

                    var bodyName = default(string);
                    var bodyType = default(string);
                    foreach (var parameter in method.ParameterList.Parameters)
                    {
                        var bodyAttribute = parameter.AttributeLists
                            .Select(x => x.Attributes)
                            .SelectMany(x => x)
                            .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequestBody");

                        if (!(bodyAttribute is null))
                        {
                            bodyName = parameter.Identifier.ValueText;
                            bodyType = parameter.Type.ToString();
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(bodyName))
                    {
                        sourceBuilder.AppendLine(3, $"return base.SendRequestAsync<{method.ReturnType.ToString().Replace("Task<", "").Replace(">", "")}, {bodyType}>(");
                        sourceBuilder.AppendLine(4, $"body: {bodyName},");
                    }
                    else
                    {
                        sourceBuilder.AppendLine(3, $"return base.SendRequestAsync<{method.ReturnType.ToString().Replace("Task<", "").Replace(">", "")}>(");
                    }
                    sourceBuilder.AppendLine(4, $"queryParameters: queryParameters,");
                    sourceBuilder.AppendLine(4, $"apiVersion: \"{apiVersion}\",");
                    sourceBuilder.AppendLine(4, $"method: \"{httpMethod}\",");
                    sourceBuilder.AppendLine(4, $"route: $\"{routeTemplate}\"");
                    sourceBuilder.AppendLine(3, ");");
                    sourceBuilder.AppendLine(2, "}");
                }

                sourceBuilder.AppendLine(1, "}");

                sourceBuilder.AppendLine("}");

                context.AddSource($"{className}.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
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