using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;

namespace Endpoint.Core.Builders
{
    public interface IResponseBaseBuilder
    {
        public void Build(Endpoint.SharedKernal.Models.Settings settings);
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
        public void Build(Endpoint.SharedKernal.Models.Settings settings)
        {
            var template = _templateLocator.Get("ResponseBase");

            var tokens = new TokensBuilder()
                .With("RootNamespace", (Token)settings.RootNamespace)
                .With("Directory", (Token)settings.DomainDirectory)
                .With("Namespace", (Token)settings.DomainNamespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{(Token)settings.DomainDirectory}/ResponseBase.cs", contents);
        }
    }
}
