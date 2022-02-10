using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;

namespace Endpoint.Core.Builders
{
    public class ProgramBuilder
    {
        public static void Build(Settings settings, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor, IFileSystem fileSystem)
        {
            var template = templateLocator.Get("Program");

            var tokens = new TokensBuilder()
                .With(nameof(settings.InfrastructureNamespace), (Token)settings.InfrastructureNamespace)
                .With("Directory", (Token)settings.ApiDirectory)
                .With("Namespace", (Token)settings.ApiNamespace)
                .With("DbContext", (Token)settings.DbContextName)
                .Build();

            var contents = templateProcessor.Process(template, tokens);

            fileSystem.WriteAllLines($@"{settings.ApiDirectory}/Program.cs", contents);
        }
    }
}
