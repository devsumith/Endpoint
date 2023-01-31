using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;

namespace Endpoint.Core.Builders;

public class ProgramBuilder
{
    public static void Build(SettingsModel settings, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor, IFileSystem fileSystem)
    {
        var template = templateLocator.Get("Program");

        var tokens = new TokensBuilder()
            .With(nameof(settings.InfrastructureNamespace), (SyntaxToken)settings.InfrastructureNamespace)
            .With("Directory", (SyntaxToken)settings.ApiDirectory)
            .With("Namespace", (SyntaxToken)settings.ApiNamespace)
            .With("DbContext", (SyntaxToken)settings.DbContextName)
            .Build();

        var contents = templateProcessor.Process(template, tokens);

        fileSystem.WriteAllLines($@"{settings.ApiDirectory}/Program.cs", contents);
    }
}
