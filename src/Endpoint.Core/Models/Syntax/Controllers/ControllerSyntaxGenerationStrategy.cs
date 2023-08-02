// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;

namespace Endpoint.Core.Models.Syntax.Controllers;

public class ControllerSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<ControllerModel>
{
    public ControllerSyntaxGenerationStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public override string Create(ISyntaxGenerator syntaxGenerator, ControllerModel model, dynamic context = null)
    {
        /*            new ClassBuilder($"{((Token)model).PascalCase}Controller", new Endpoint.Core.Services.Context(), fileSystem)
                .WithDirectory($"{settings.ApiDirectory}{Path.DirectorySeparatorChar}Controllers")
                .WithUsing("System.Net")
                .WithUsing("System.Threading")
                .WithUsing("System.Threading.Tasks")
                .WithUsing($"{settings.ApplicationNamespace}")
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("Microsoft.AspNetCore.Mvc")
                .WithUsing("Microsoft.Extensions.Logging")
                .WithUsing("Swashbuckle.AspNetCore.Annotations")
                .WithUsing("System.Net.Mime")
                .WithNamespace($"{settings.ApiNamespace}.Controllers")
                .WithAttribute(new GenericAttributeGenerationStrategy().WithName("ApiController").Build())
                .WithAttribute(new GenericAttributeGenerationStrategy().WithName("Route").WithParam("\"api/[controller]\"").Build())
                .WithAttribute(new GenericAttributeGenerationStrategy().WithName("Produces").WithParam("MediaTypeNames.Application.Json").Build())
                .WithAttribute(new GenericAttributeGenerationStrategy().WithName("Consumes").WithParam("MediaTypeNames.Application.Json").Build())
                .WithDependency("IMediator", "mediator")
                .WithDependency($"ILogger<{((Token)model).PascalCase}Controller>", "logger")
                .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.GetById).WithResource(model).WithAuthorize(false).Build())
                .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Get).WithResource(model).WithAuthorize(false).Build())
                .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Create).WithResource(model).WithAuthorize(false).Build())
                .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Page).WithResource(model).WithAuthorize(false).Build())
                .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Update).WithResource(model).WithAuthorize(false).Build())
                .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Delete).WithResource(model).WithAuthorize(false).Build())
                .Build();
        */
        return "";
    }


}

