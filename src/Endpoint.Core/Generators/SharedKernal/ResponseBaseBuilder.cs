using Endpoint.Core.Models.Options;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;

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
                .With("RootNamespace", (Token)settings.RootNamespace)
                .With("Directory", (Token)settings.DomainDirectory)
                .With("Namespace", (Token)settings.DomainNamespace)
                .Build();

            var contents = string.Join(Environment.NewLine,_templateProcessor.Process(template, tokens));

            _fileSystem.WriteAllText($@"{(Token)settings.DomainDirectory}/ResponseBase.cs", contents);
        }
    }
}
