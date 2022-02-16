using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.IO;

namespace Endpoint.Core.Builders
{
    public class QueryableExtensionsBuilder
    {
        public static void Build(Settings settings, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor, IFileSystem fileSystem, string directory)
        {
            var template = templateLocator.Get("QueryableExtensions");

            var tokens = new TokensBuilder()
                .With(nameof(settings.RootNamespace), (Token)settings.RootNamespace)
                .With(directory, (Token)directory)
                .With("Namespace", (Token)settings.ApplicationNamespace)
                .With(nameof(settings.ApplicationNamespace), (Token)settings.ApplicationNamespace)
                .Build();

            var contents = templateProcessor.Process(template, tokens);

            fileSystem.WriteAllLines($@"{directory}{Path.DirectorySeparatorChar}QueryableExtensions.cs", contents);
        }
    }
}
