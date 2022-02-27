using Endpoint.Core.Enums;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.IO;

namespace Endpoint.Core.Builders
{
    public class ControllerBuilder
    {
        public static void Default(Models.Settings settings, string resource, IFileSystem fileSystem)
        {
            new ClassBuilder($"{((Token)resource).PascalCase}Controller", new Endpoint.Core.Services.Context(), fileSystem)
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
            .WithDependency($"ILogger<{((Token)resource).PascalCase}Controller>", "logger")
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.GetById).WithResource(resource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.Get).WithResource(resource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.Create).WithResource(resource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.Page).WithResource(resource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.Update).WithResource(resource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.Delete).WithResource(resource).WithAuthorize(false).Build())
            .Build();
        }
    }
}
