using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class QueryableExtensionsBuilder
    {


        public static void Build(Settings settings, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor, IFileSystem fileSystem, string directory)
        {
            var template = templateLocator.Get(nameof(QueryableExtensionsBuilder));

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
