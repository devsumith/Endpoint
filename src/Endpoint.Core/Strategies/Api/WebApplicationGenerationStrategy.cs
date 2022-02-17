using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Api
{
    public interface IWebApplicationGenerationStrategy
    {
        string[] Create(MinimalApiProgramModel model);
    }

    public class WebApplicationGenerationStrategy: IWebApplicationGenerationStrategy
    {
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;
        public WebApplicationGenerationStrategy(ITemplateProcessor templateProcessor, ITemplateLocator templateLocator)
        {
            _templateLocator = templateLocator;
            _templateProcessor = templateProcessor;
        }

        public string[] Create(MinimalApiProgramModel model)
        {
            var tokens = new TokensBuilder()
                .With(nameof(model.ApiNamespace), (Token)model.ApiNamespace)
                .With(nameof(model.DbContextName), (Token)model.DbContextName)
                .Build();

            List<string> content = new List<string>();

            content.Add("var app = builder.Build();");

            content.Add("");

            content.AddRange(_templateProcessor.Process(_templateLocator.Get("WebApplicationConfiguration"), tokens));

            content.Add("");

            foreach(var routeHandlerModel in model.RouteHandlers)
            {
                content.AddRange(new RouteHandlerGenerationStrategy().Create(routeHandlerModel));

                content.Add("");
            }

            
            content.Add("app.Run();");

            return content.ToArray();

        }
    }
}
