using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mittons.Azure.Devops.Extension.SourceGenerator.Extensions
{
    internal static class InterfaceDeclarationSyntaxExtensions
    {
        public static string GetNamespace(this InterfaceDeclarationSyntax @syntax)
        {
            var result = string.Empty;

            var potentialNamespaceParent = @syntax.Parent;

            while (potentialNamespaceParent != null &&
                !typeof(NamespaceDeclarationSyntax).IsAssignableFrom(potentialNamespaceParent.GetType()) &&
                !typeof(FileScopedNamespaceDeclarationSyntax).IsAssignableFrom(potentialNamespaceParent.GetType()))
            {
                potentialNamespaceParent = potentialNamespaceParent.Parent;
            }

            if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
            {
                result = namespaceParent.Name.ToString();

                while (true)
                {
                    if (!typeof(NamespaceDeclarationSyntax).IsAssignableFrom(potentialNamespaceParent.Parent.GetType()))
                    {
                        break;
                    }

                    result = $"{namespaceParent.Name}.{result}";
                    namespaceParent = namespaceParent.Parent as NamespaceDeclarationSyntax;
                }
            }

            return result;
        }
    }
}