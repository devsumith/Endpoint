using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;

namespace Endpoint.Core.Strategies.Api
{

    public class WebApplicationBuilderGenerationStrategy: IWebApplicationBuilderGenerationStrategy
    {
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;

        public WebApplicationBuilderGenerationStrategy(ITemplateProcessor templateProcessor, ITemplateLocator templateLocator)
        {
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
        }

        public string[] Create(string @namespace, string dbContextName)
        {
            var template = _templateLocator.Get("WebApplicationBuilder");

            var tokens = new TokensBuilder()
                .With("Namespace", (Token)@namespace)
                .With(nameof(dbContextName), (Token)dbContextName)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            return contents;
        }
    }
}
