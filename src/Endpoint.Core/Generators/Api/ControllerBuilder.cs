using Endpoint.Core.Enums;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Builders;

public class ControllerBuilder
{
    public static void Default(SettingsModel settings, string resource, IFileSystem fileSystem)
    {
        new ClassBuilder($"{((SyntaxToken)resource).PascalCase}Controller", new Endpoint.Core.Services.Context(), fileSystem)
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
        .WithDependency($"ILogger<{((SyntaxToken)resource).PascalCase}Controller>", "logger")
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.GetById).WithResource(resource).WithAuthorize(false).Build())
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Get).WithResource(resource).WithAuthorize(false).Build())
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Create).WithResource(resource).WithAuthorize(false).Build())
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Page).WithResource(resource).WithAuthorize(false).Build())
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Update).WithResource(resource).WithAuthorize(false).Build())
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Delete).WithResource(resource).WithAuthorize(false).Build())
        .Build();
    }
}
