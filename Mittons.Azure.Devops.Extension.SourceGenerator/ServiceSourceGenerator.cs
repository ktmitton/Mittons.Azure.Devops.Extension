using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Mittons.Azure.Devops.Extension.SourceGenerator
{
    [Generator]
    public class ServiceSourceGenerator : ISourceGenerator
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
                    .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "GenerateService");

                var contributionId = serviceModel.GetConstantValue(serviceAttribute.ArgumentList.Arguments[0].Expression).ToString();

                var methods = ids.DescendantNodes().OfType<MethodDeclarationSyntax>();
                var sourceBuilder = new StringBuilder();
                var interfaceName = ids.Identifier.ValueText;
                var className = ids.Identifier.ValueText.Substring(1);
                sourceBuilder.Append("using System.Text.Json.Serialization;\n");
                sourceBuilder.Append("using Microsoft.Extensions.DependencyInjection;\n");
                sourceBuilder.Append("using Mittons.Azure.Devops.Extension.Xdm;\n");
                sourceBuilder.Append("using Mittons.Azure.Devops.Extension.Models;\n");
                sourceBuilder.Append("\n");
                sourceBuilder.Append("#nullable enable\n");
                sourceBuilder.Append("\n");
                sourceBuilder.Append("namespace Mittons.Azure.Devops.Extension.Service\n");
                sourceBuilder.Append("{\n");



                sourceBuilder.Append($"\tinternal static class {className}Extensions\n");
                sourceBuilder.Append("\t{\n");
                sourceBuilder.Append($"\t\tpublic static IServiceCollection Add{className}(this IServiceCollection @serviceCollection)\n");
                sourceBuilder.Append($"\t\t\t=> @serviceCollection.AddSingleton<{ids.Identifier.ValueText}, {className}>();\n");
                sourceBuilder.Append("\t}\n");
                sourceBuilder.Append("\n");

                sourceBuilder.Append($"\tinternal class {className} : {interfaceName}\n");
                sourceBuilder.Append("\t{\n");
                sourceBuilder.Append($"\t\tprivate const string ContributionId = \"{contributionId}\";\n");
                sourceBuilder.Append("\n");
                sourceBuilder.Append("\t\tprivate readonly IChannel _channel;\n");
                sourceBuilder.Append("\n");
                sourceBuilder.Append("\t\tprivate readonly Task _ready;\n");
                sourceBuilder.Append("\n");
                sourceBuilder.Append($"\t\tprivate {className}Definition? _definition;\n");
                sourceBuilder.Append("\n");
                sourceBuilder.Append($"\t\tpublic {className}(IChannel channel)\n");
                sourceBuilder.Append("\t\t{\n");
                sourceBuilder.Append("\t\t\t_channel = channel;\n");
                sourceBuilder.Append("\t\t\t_ready = InitializeAsync();\n");
                sourceBuilder.Append("\t\t}\n");
                sourceBuilder.Append("\n");
                sourceBuilder.Append("\t\tprivate async Task InitializeAsync()\n");
                sourceBuilder.Append("\t\t{\n");
                sourceBuilder.Append($"\t\t\t_definition = await _channel.GetServiceDefinitionAsync<{className}Definition>(ContributionId);\n");
                sourceBuilder.Append("\n");
                sourceBuilder.Append($"\t\t\tif (_definition is null)\n");
                sourceBuilder.Append("\t\t\t{\n");
                sourceBuilder.Append($"\t\t\t\tthrow new NullReferenceException($\"Unabled to get service definition for {{ContributionId}}\");\n");
                sourceBuilder.Append("\t\t\t}\n");

                foreach (var method in methods)
                {
                    var methodName = method.Identifier.Text;
                    sourceBuilder.Append("\n");
                    sourceBuilder.Append($"\t\t\tif (_definition.{methodName}_ProxyFunctionDefinition is null)\n");
                    sourceBuilder.Append("\t\t\t{\n");
                    sourceBuilder.Append($"\t\t\t\tthrow new NullReferenceException($\"Service definition did not include required proxy function definition for {{{className}Definition.{methodName}_PropertyName}}\");\n");
                    sourceBuilder.Append("\t\t\t}\n");
                }
                sourceBuilder.Append("\t\t}\n");


                foreach (var method in methods)
                {
                    var model = context.Compilation.GetSemanticModel(method.SyntaxTree);
                    var methodName = method.Identifier.Text;
                    var proxyFunctionAttribute = method.AttributeLists
                        .Select(x => x.Attributes)
                        .SelectMany(x => x)
                        .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ProxyFunction");

                    var functionName = model.GetConstantValue(proxyFunctionAttribute.ArgumentList.Arguments[0].Expression).ToString();

                    sourceBuilder.Append("\n");
                    sourceBuilder.Append($"\t\tpublic async {method.ReturnType.ToString()} {methodName}");
                    if (method.TypeParameterList?.Parameters.Any() ?? false)
                    {
                        sourceBuilder.Append($"<{string.Join(", ", method.TypeParameterList.Parameters.Select(x => x.Identifier.ValueText))}>");
                    }
                    sourceBuilder.Append($"({string.Join(", ", method.ParameterList.Parameters.Select(x => $"{x.Type} {x.Identifier.ValueText}"))})\n");
                    sourceBuilder.Append("\t\t{\n");
                    sourceBuilder.Append("\t\t\tawait _ready;\n");
                    sourceBuilder.Append("\n");
                    var innerType = method.ReturnType.ToString().Replace("Task", "");
                    if (string.IsNullOrWhiteSpace(innerType))
                    {
                        sourceBuilder.Append($"\t\t\tawait _channel.InvokeRemoteProxyMethodAsync(_definition?.{methodName}_ProxyFunctionDefinition");
                    }
                    else
                    {
                        sourceBuilder.Append($"\t\t\treturn await _channel.InvokeRemoteProxyMethodAsync{innerType}(_definition?.{methodName}_ProxyFunctionDefinition");
                    }
                    for (var i = 0; i < method.ParameterList.Parameters.Count; i++)
                    {
                        var parameter = method.ParameterList.Parameters[i];
                        var name = parameter.Identifier.ValueText;
                        sourceBuilder.Append($", {name}");
                    }
                    sourceBuilder.Append(");\n");
                    sourceBuilder.Append("\t\t}\n");
                }
                sourceBuilder.Append("\t}\n");
                sourceBuilder.Append("\n");

                sourceBuilder.Append($"\tinternal record {className}Definition\n");
                sourceBuilder.Append("\t{");

                foreach (var method in methods)
                {
                    var methodName = method.Identifier.Text;
                    var model = context.Compilation.GetSemanticModel(method.SyntaxTree);
                    var proxyFunctionAttribute = method.AttributeLists
                        .Select(x => x.Attributes)
                        .SelectMany(x => x)
                        .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ProxyFunction");

                    var proxyName = model.GetConstantValue(proxyFunctionAttribute.ArgumentList.Arguments[0].Expression).ToString();

                    sourceBuilder.Append("\n");
                    sourceBuilder.Append($"\t\tpublic const string {methodName}_PropertyName = \"{proxyName}\";\n");
                    sourceBuilder.Append("\n");
                    sourceBuilder.Append($"\t\t[JsonPropertyName({methodName}_PropertyName)]\n");
                    sourceBuilder.Append($"\t\tpublic ProxyFunctionDefinition? {methodName}_ProxyFunctionDefinition {{ get; init; }}\n");
                }
                sourceBuilder.Append("\t}\n");
                sourceBuilder.Append("}\n");

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
                    .Any(x => x.Identifier.ValueText == "GenerateService");

                if (requiresGeneration)
                {
                    DecoratorRequestingInterfaces.Add(ids);
                }
            }
        }
    }
}