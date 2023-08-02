// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.RouteHandlers;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Api
{

    public class WebApplicationGenerationStrategy : IWebApplicationGenerationStrategy
    {
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;
        public WebApplicationGenerationStrategy(ITemplateProcessor templateProcessor, ITemplateLocator templateLocator)
        {
            _templateLocator = templateLocator;
            _templateProcessor = templateProcessor;
        }

        public string Create(string @namespace, string dbContextName, List<RouteHandlerModel> routeHandlers)
        {
            var tokens = new TokensBuilder()
                .With("Namespace", (SyntaxToken)@namespace)
                .With(nameof(dbContextName), (SyntaxToken)dbContextName)
                .Build();

            List<string> content = new List<string>
            {
                "var app = builder.Build();",

                "",

                string.Join(Environment.NewLine, _templateProcessor.Process(_templateLocator.Get("WebApplicationConfiguration"), tokens)),

                ""
            };

            foreach (var routeHandlerModel in routeHandlers)
            {
                content.Add(new RouteHandlerGenerationStrategy().Create(routeHandlerModel));

                content.Add("");
            }

            content.Add("app.Run();");

            return string.Join(Environment.NewLine, content);

        }

        public string Update(List<string> existingWebApplication, string @namespace, string dbContextName, List<RouteHandlerModel> routeHandlers)
        {
            var tokens = new TokensBuilder()
                .With("Namespace", (SyntaxToken)@namespace)
                .With(nameof(dbContextName), (SyntaxToken)dbContextName)
                .Build();

            List<string> content = new List<string>
            {
                "var app = builder.Build();",

                "",

                string.Join(Environment.NewLine, _templateProcessor.Process(_templateLocator.Get("WebApplicationConfiguration"), tokens)),

                ""
            };

            foreach (var routeHandlerModel in routeHandlers)
            {
                content.Add(new RouteHandlerGenerationStrategy().Create(routeHandlerModel));

                content.Add("");
            }

            content.Add("app.Run();");

            return string.Join(Environment.NewLine, content);

        }
    }
}

