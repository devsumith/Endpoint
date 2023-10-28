// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Documents;
using Endpoint.Core.Syntax.Properties;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Units.Factories;

public class DocumentFactory : IDocumentFactory
{
    private readonly ILogger<DocumentFactory> logger;
    private readonly IClassFactory classFactory;
    private readonly IContext context;

    public DocumentFactory(ILogger<DocumentFactory> logger, IClassFactory classFactory, IContext context)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DocumentModel> CreateCommandAsync(ClassModel aggregate, RouteType routeType)
    {
        logger.LogInformation("Creating Command Document. {aggregateName}", aggregate.Name);

        var context = this.context.Get<DocumentModel>();

        var model = new DocumentModel()
        {
            RootNamespace = context.RootNamespace,
        };

        model.Namespace = $"AggregatesModel.{aggregate.Name}Aggregate.Commands";

        switch (routeType)
        {
            case RouteType.Create:
                model.Name = $"Create{aggregate.Name}";

                var requestModel = await classFactory.CreateRequestAsync($"{model.Name}Request", $"{model.Name}Request", aggregate.Properties.Where(x => x.Name != $"{aggregate.Name}Id").ToList());

                model.Code.Add(requestModel);

                model.Code.Add(new ClassModel($"{model.Name}Validator")
                {
                    Usings = new List<UsingModel>()
                    {
                        new UsingModel("FluentValidation"),
                    },
                });

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Response", new List<PropertyModel>()));

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Handler", new List<PropertyModel>()));

                break;

            case RouteType.Delete:
                model.Name = $"Delete{aggregate.Name}";

                model.Code.Add(new ClassModel($"{model.Name}Validator")
                {
                    Usings = new List<UsingModel>()
                    {
                        new UsingModel("FluentValidation"),
                    },
                });

                model.Code.Add(await classFactory.CreateRequestAsync($"{model.Name}Request", $"{model.Name}Request", aggregate.Properties.Where(x => x.Name == $"{aggregate.Name}Id").ToList()));

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Response", new List<PropertyModel>()));

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Handler", new List<PropertyModel>()));

                break;

            case RouteType.Update:
                model.Name = $"Update{aggregate.Name}";

                model.Code.Add(new ClassModel($"{model.Name}Validator")
                {
                    Usings = new List<UsingModel>()
                    {
                        new UsingModel("FluentValidation"),
                    },
                });

                model.Code.Add(await classFactory.CreateRequestAsync($"{model.Name}Request", $"{model.Name}Request", aggregate.Properties));

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Response", new List<PropertyModel>()));

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Handler", new List<PropertyModel>()));

                break;
        }

        return model;
    }

    public async Task<DocumentModel> CreateQueryAsync(ClassModel aggregate, RouteType routeType)
    {
        logger.LogInformation("Creating Query Document. {aggregateName}", aggregate.Name);

        var context = this.context.Get<DocumentModel>();

        var model = new DocumentModel()
        {
            RootNamespace = context.RootNamespace,
        };

        model.Namespace = $"AggregatesModel.{aggregate.Name}Aggregate.Commands";

        switch (routeType)
        {
            case RouteType.Get:
                model.Name = $"Create{aggregate.Name}";

                var requestModel = await classFactory.CreateRequestAsync($"{model.Name}Request", $"{model.Name}Request", aggregate.Properties.Where(x => x.Name != $"{aggregate.Name}Id").ToList());

                model.Code.Add(requestModel);

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Response", new List<PropertyModel>()));

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Handler", new List<PropertyModel>()));

                break;

            case RouteType.GetById:
                model.Name = $"Delete{aggregate.Name}";

                model.Code.Add(await classFactory.CreateRequestAsync($"{model.Name}Request", $"{model.Name}Request", aggregate.Properties.Where(x => x.Name == $"{aggregate.Name}Id").ToList()));

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Response", new List<PropertyModel>()));

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Handler", new List<PropertyModel>()));

                break;

            case RouteType.Page:
                model.Name = $"Update{aggregate.Name}";

                model.Code.Add(await classFactory.CreateRequestAsync($"{model.Name}Request", $"{model.Name}Request", aggregate.Properties));

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Response", new List<PropertyModel>()));

                model.Code.Add(await classFactory.CreateResponseAsync($"{model.Name}Handler", new List<PropertyModel>()));

                break;
        }

        return model;
    }
}
