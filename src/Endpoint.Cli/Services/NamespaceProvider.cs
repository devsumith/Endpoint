using Endpoint.Cli.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Cli.Services
{
    public class NamespaceProvider : INamespaceProvider
    {
        private readonly CliSettings _settings;
        public NamespaceProvider(ISettingsProvider settingsProvider)
        {
            _settings = settingsProvider.Get();
        }
        public string GetFileNamespace(string path, List<string> fileNamespaceParts = default)
        {
            string @namespace = _settings.Namespace;

            var root = File.Exists($@"{path}/cliSettings.json");

            if (root)
            {
                if (fileNamespaceParts == default)
                    return @namespace;

                fileNamespaceParts.Reverse();

                return $"{@namespace}.{string.Join(".", fileNamespaceParts)}";
            }

            if (fileNamespaceParts == default)
            {
                fileNamespaceParts = new List<string>();
            }

            var parts = path.Split(Path.DirectorySeparatorChar);

            if (parts.Length == 1)
                return "Fail";

            fileNamespaceParts.Add(parts.Last());

            return GetFileNamespace(string.Join($"{Path.DirectorySeparatorChar}", parts.Take(parts.Length - 1)), fileNamespaceParts);

        }
    }
}