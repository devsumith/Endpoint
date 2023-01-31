using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;

namespace Endpoint.Core.Builders
{
    public interface IResponseBaseBuilder
    {
        public void Build(SettingsModel settings);
    }
    public class ResponseBaseBuilder : IResponseBaseBuilder
    {
        protected readonly ICommandService _commandService;
        protected readonly ITemplateProcessor _templateProcessor;
        protected readonly ITemplateLocator _templateLocator;
        protected readonly IFileSystem _fileSystem;

        public ResponseBaseBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem)
        {
            _commandService = commandService;
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
            _fileSystem = fileSystem;
        }
        public void Build(SettingsModel settings)
        {
            var template = _templateLocator.Get("ResponseBase");

            var tokens = new TokensBuilder()
                .With("RootNamespace", (SyntaxToken)settings.RootNamespace)
                .With("Directory", (SyntaxToken)settings.DomainDirectory)
                .With("Namespace", (SyntaxToken)settings.DomainNamespace)
                .Build();

            var contents = string.Join(Environment.NewLine,_templateProcessor.Process(template, tokens));

            _fileSystem.WriteAllText($@"{(SyntaxToken)settings.DomainDirectory}/ResponseBase.cs", contents);
        }
    }
}
