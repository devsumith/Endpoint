using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;

namespace Endpoint.Core.Strategies.Api
{
    public interface IWebApplicationBuilderGenerationStrategy
    {
        string[] Create(MinimalApiProgramModel model);
    }

    public class WebApplicationBuilderGenerationStrategy: IWebApplicationBuilderGenerationStrategy
    {
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;

        public WebApplicationBuilderGenerationStrategy(ITemplateProcessor templateProcessor, ITemplateLocator templateLocator)
        {
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
        }

        public string[] Create(MinimalApiProgramModel model)
        {
            var template = _templateLocator.Get("WebApplicationBuilder");

            var tokens = new TokensBuilder()
                .With(nameof(model.ApiNamespace), (Token)model.ApiNamespace)
                .With(nameof(model.DbContextName), (Token)model.DbContextName)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            return contents;
        }
    }
}
