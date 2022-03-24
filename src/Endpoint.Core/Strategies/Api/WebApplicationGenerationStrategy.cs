using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Api
{

    public class WebApplicationGenerationStrategy: IWebApplicationGenerationStrategy
    {
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;
        public WebApplicationGenerationStrategy(ITemplateProcessor templateProcessor, ITemplateLocator templateLocator)
        {
            _templateLocator = templateLocator;
            _templateProcessor = templateProcessor;
        }

        public string[] Create(string @namespace, string dbContextName, List<RouteHandlerModel> routeHandlers)
        {
            var tokens = new TokensBuilder()
                .With("Namespace", (Token)@namespace)
                .With(nameof(dbContextName), (Token)dbContextName)
                .Build();

            List<string> content = new List<string>();

            content.Add("var app = builder.Build();");

            content.Add("");

            content.AddRange(_templateProcessor.Process(_templateLocator.Get("WebApplicationConfiguration"), tokens));

            content.Add("");

            foreach(var routeHandlerModel in routeHandlers)
            {
                content.AddRange(new RouteHandlerGenerationStrategy().Create(routeHandlerModel));

                content.Add("");
            }

            content.Add("app.Run();");

            return content.ToArray();

        }
    }
}
