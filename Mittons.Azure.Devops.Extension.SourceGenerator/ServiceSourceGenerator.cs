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
                sourceBuilder.AppendLine("using System.Text.Json.Serialization;");
                sourceBuilder.AppendLine("using Microsoft.Extensions.DependencyInjection;");
                sourceBuilder.AppendLine("using Mittons.Azure.Devops.Extension.Sdk;");
                sourceBuilder.AppendLine("using Mittons.Azure.Devops.Extension.Xdm;");
                sourceBuilder.AppendLine("using Mittons.Azure.Devops.Extension.Models;");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine("#nullable enable");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine("namespace Mittons.Azure.Devops.Extension.Service");
                sourceBuilder.AppendLine("{");



                sourceBuilder.AppendLine(1, $"internal static class {className}Extensions");
                sourceBuilder.AppendLine(1, "{");
                sourceBuilder.AppendLine(2, $"public static IServiceCollection Add{className}(this IServiceCollection @serviceCollection)");
                sourceBuilder.AppendLine(3, $"=> @serviceCollection.AddSingleton<{ids.Identifier.ValueText}, {className}>();");
                sourceBuilder.AppendLine(1, "}");
                sourceBuilder.AppendLine();

                sourceBuilder.AppendLine(1, $"internal class {className} : {interfaceName}");
                sourceBuilder.AppendLine(1, "{");
                sourceBuilder.AppendLine(2, $"private const string ContributionId = \"{contributionId}\";");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine(2, "private readonly IChannel _channel;");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine(2, "private readonly Task _ready;");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine(2, $"private {className}Definition? _definition;");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine(2, $"public {className}(ISdk sdk, IChannel channel)");
                sourceBuilder.AppendLine(2, "{");
                sourceBuilder.AppendLine(3, "_channel = channel;");
                sourceBuilder.AppendLine(3, "_ready = InitializeAsync(sdk);");
                sourceBuilder.AppendLine(2, "}");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine(2, "private async Task InitializeAsync(ISdk sdk)");
                sourceBuilder.AppendLine(2, "{");
                sourceBuilder.AppendLine(3, "await sdk.Ready;");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine(3, $"_definition = await _channel.GetServiceDefinitionAsync<{className}Definition>(ContributionId);");
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine(3, $"if (_definition is null)");
                sourceBuilder.AppendLine(3, "{");
                sourceBuilder.AppendLine(4, $"throw new NullReferenceException($\"Unabled to get service definition for {{ContributionId}}\");");
                sourceBuilder.AppendLine(3, "}");

                foreach (var method in methods)
                {
                    var methodName = method.Identifier.Text;
                    sourceBuilder.AppendLine();
                    sourceBuilder.AppendLine(3, $"if (_definition.{methodName}_ProxyFunctionDefinition is null)");
                    sourceBuilder.AppendLine(3, "{");
                    sourceBuilder.AppendLine(4, $"throw new NullReferenceException($\"Service definition did not include required proxy function definition for {{{className}Definition.{methodName}_PropertyName}}\");");
                    sourceBuilder.AppendLine(3, "}");
                }
                sourceBuilder.AppendLine(2, "}");


                foreach (var method in methods)
                {
                    var model = context.Compilation.GetSemanticModel(method.SyntaxTree);
                    var methodName = method.Identifier.Text;
                    var proxyFunctionAttribute = method.AttributeLists
                        .Select(x => x.Attributes)
                        .SelectMany(x => x)
                        .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ProxyFunction");

                    var functionName = model.GetConstantValue(proxyFunctionAttribute.ArgumentList.Arguments[0].Expression).ToString();

                    sourceBuilder.AppendLine();
                    sourceBuilder.Append(2, $"public async {method.ReturnType.ToString()} {methodName}");
                    if (method.TypeParameterList?.Parameters.Any() ?? false)
                    {
                        sourceBuilder.Append($"<{string.Join(", ", method.TypeParameterList.Parameters.Select(x => x.Identifier.ValueText))}>");
                    }
                    sourceBuilder.AppendLine($"({string.Join(", ", method.ParameterList.Parameters.Select(x => $"{x.Type} {x.Identifier.ValueText}"))})");
                    sourceBuilder.AppendLine(2, "{");
                    sourceBuilder.AppendLine(3, "await _ready;");
                    sourceBuilder.AppendLine();
                    var innerType = method.ReturnType.ToString().Replace("Task", "");
                    if (string.IsNullOrWhiteSpace(innerType))
                    {
                        sourceBuilder.Append(3, $"await _channel.InvokeRemoteProxyMethodAsync(_definition?.{methodName}_ProxyFunctionDefinition");
                    }
                    else
                    {
                        sourceBuilder.Append(3, $"return await _channel.InvokeRemoteProxyMethodAsync{innerType}(_definition?.{methodName}_ProxyFunctionDefinition");
                    }
                    for (var i = 0; i < method.ParameterList.Parameters.Count; i++)
                    {
                        var parameter = method.ParameterList.Parameters[i];
                        var name = parameter.Identifier.ValueText;
                        sourceBuilder.Append($", {name}");
                    }
                    sourceBuilder.AppendLine(");");
                    sourceBuilder.AppendLine(2, "}");
                }
                sourceBuilder.AppendLine(1, "}");
                sourceBuilder.AppendLine();

                sourceBuilder.AppendLine(1, $"internal record {className}Definition");
                sourceBuilder.AppendLine(1, "{");

                var flag = true;

                foreach (var method in methods)
                {
                    var methodName = method.Identifier.Text;
                    var model = context.Compilation.GetSemanticModel(method.SyntaxTree);
                    var proxyFunctionAttribute = method.AttributeLists
                        .Select(x => x.Attributes)
                        .SelectMany(x => x)
                        .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ProxyFunction");

                    var proxyName = model.GetConstantValue(proxyFunctionAttribute.ArgumentList.Arguments[0].Expression).ToString();

                    if (!flag)
                    {
                        sourceBuilder.AppendLine();
                    }
                    else
                    {
                        flag = false;
                    }

                    sourceBuilder.AppendLine(2, $"public const string {methodName}_PropertyName = \"{proxyName}\";");
                    sourceBuilder.AppendLine(2, $"[JsonPropertyName({methodName}_PropertyName)]");
                    sourceBuilder.AppendLine(2, $"public ProxyFunctionDefinition? {methodName}_ProxyFunctionDefinition {{ get; init; }}");
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
                    .Any(x => x.Identifier.ValueText == "GenerateService");

                if (requiresGeneration)
                {
                    DecoratorRequestingInterfaces.Add(ids);
                }
            }
        }
    }
}