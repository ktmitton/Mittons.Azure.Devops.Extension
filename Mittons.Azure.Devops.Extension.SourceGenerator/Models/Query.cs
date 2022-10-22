using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mittons.Azure.Devops.Extension.SourceGenerator.Models
{
    internal class Query
    {
        public List<Property> Parameters { get; set; }

        public Query(MethodDeclarationSyntax method)
        {
            Parameters = new List<Property>();

            foreach (var parameter in method.ParameterList.Parameters)
            {
                var queryAttribute = parameter.AttributeLists
                    .Select(x => x.Attributes)
                    .SelectMany(x => x)
                    .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequestQueryParameter");

                if (!(queryAttribute is null))
                {
                    Parameters.Add(new Property(parameter.Identifier.ValueText));
                }
            }
        }
    }
}
