using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mittons.Azure.Devops.Extension.SourceGenerator.Models
{
    internal class RequestBody
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

        public RequestBody(SemanticModel serviceModel, MethodDeclarationSyntax method)
        {
            var clientRequestAttribute = method.AttributeLists
                .Select(x => x.Attributes)
                .SelectMany(x => x)
                .Single(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientRequest");

            foreach (var parameter in method.ParameterList.Parameters)
            {
                var byteArrayBodyAttribute = parameter.AttributeLists
                    .Select(x => x.Attributes)
                    .SelectMany(x => x)
                    .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientByteArrayRequestBodyAttribute");

                if (!(byteArrayBodyAttribute is null))
                {
                    IsByteArrayBody = true;

                    ByteArrayParameter = parameter.Identifier.ValueText;
                }

                var jsonBodyAttribute = parameter.AttributeLists
                    .Select(x => x.Attributes)
                    .SelectMany(x => x)
                    .SingleOrDefault(x => (x.Name is IdentifierNameSyntax ins) && ins.Identifier.ValueText == "ClientJsonRequestBodyParameterAttribute");

                if (!(jsonBodyAttribute is null))
                {
                    IsJsonBody = true;

                    JsonProperties.Add(new Property(parameter.Identifier.ValueText));
                }
            }

            if (clientRequestAttribute.ArgumentList.Arguments.Count() > 4)
            {
                ContentType = serviceModel.GetConstantValue(clientRequestAttribute.ArgumentList.Arguments[4].Expression).ToString();
            }
            else if (IsJsonBody)
            {
                ContentType = "application/json";
            }
            else if (IsByteArrayBody)
            {
                ContentType = MediaTypeNames.Application.Octet;
            }
        }
    }
}
