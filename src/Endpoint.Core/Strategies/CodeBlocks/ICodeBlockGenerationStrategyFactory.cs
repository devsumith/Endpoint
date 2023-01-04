using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;
using Endpoint.Core.ViewModels;
using System;

namespace Endpoint.Core.Strategies.CodeBlocks
{
    public interface ICodeBlockGenerationStrategyFactory
    {
        string CreateFor(dynamic model);
    }

    public interface ICodeBlockGenerationStrategy
    {
        int Order { get; set; }
        bool CanHandle(dynamic model);
        string Create(dynamic model);
    }

    public class CodeBlockGenerationStrategy : ICodeBlockGenerationStrategy
    {
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;

        public CodeBlockGenerationStrategy(IFileSystem fileSystem, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor)
        {
            _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        }
        public int Order { get; set; } = 0;

        public bool CanHandle(dynamic model) => model is EntityModel;

        public string Create(dynamic model) => Create(model);

        public string Create(EntityModel model)
        {
            var template = _templateLocator.Get(nameof(EntityModel));

            var viewModel = model.ToViewModel();

            return _templateProcessor.Process(template, viewModel);
        }
    }

    public class EntityCodeBlockGenerationStrategy: ICodeBlockGenerationStrategy
    {
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;

        public EntityCodeBlockGenerationStrategy(IFileSystem fileSystem, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor)
        {
            _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        }
        public int Order { get; set; } = 0;

        public bool CanHandle(dynamic model) => model is EntityModel;

        public string Create(dynamic model) => Create(model);

        public string Create(EntityModel model)
        {
            var template = _templateLocator.Get(nameof(EntityModel));

            var viewModel = model.ToViewModel();

            return _templateProcessor.Process(template, viewModel);
        }
    }
}
