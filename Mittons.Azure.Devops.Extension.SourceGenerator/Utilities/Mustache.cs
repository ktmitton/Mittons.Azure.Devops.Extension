using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HandlebarsDotNet;

namespace Mittons.Azure.Devops.Extension.SourceGenerator.Utilities
{
    public static class Mustache
    {
        public static HandlebarsTemplate<object, object> CompileEnvironment<TGenerator>(string path, string name)
        {
            var environment = Handlebars.Create();

            var assembly = Assembly.GetAssembly(typeof(TGenerator));

            var escapedPath = path.Replace(".", @"\.");

            var template = assembly.GetManifestResourceNames().Single(x => Regex.IsMatch(x, $"{path}\\.{name}\\.mustache"));
            var partials = assembly.GetManifestResourceNames().Where(x => Regex.IsMatch(x, $"{path}\\.Partials\\.[^\\.]*\\.mustache"));

            foreach (var resource in partials)
            {
                var partialName = Regex.Replace(resource, @".*\.([^\.]+)\.mustache", "$1");

                using (var resourceStream = assembly.GetManifestResourceStream(resource))
                using (var reader = new StreamReader(resourceStream))
                {
                    environment.RegisterTemplate(partialName, reader.ReadToEnd());
                }
            }

            using (var resourceStream = assembly.GetManifestResourceStream(template))
            using (var reader = new StreamReader(resourceStream))
            {
                return environment.Compile(reader.ReadToEnd());
            }
        }
    }
}