using Endpoint.Application.Enums;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class ControllerBuilder
    {
        public static void Default(Endpoint.SharedKernal.Models.Settings settings, string resource, IFileSystem fileSystem)
        {
            new ClassBuilder($"{((Token)resource).PascalCase}Controller", new Endpoint.SharedKernal.Services.Context(), fileSystem)
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
            .WithAttribute(new AttributeBuilder().WithName("ApiController").Build())
            .WithAttribute(new AttributeBuilder().WithName("Route").WithParam("\"api/[controller]\"").Build())
            .WithAttribute(new AttributeBuilder().WithName("Produces").WithParam("MediaTypeNames.Application.Json").Build())
            .WithAttribute(new AttributeBuilder().WithName("Consumes").WithParam("MediaTypeNames.Application.Json").Build())
            .WithDependency("IMediator", "mediator")
            .WithDependency($"ILogger<{((Token)resource).PascalCase}Controller>", "logger")
            .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.GetById).WithResource(resource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Get).WithResource(resource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Create).WithResource(resource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Page).WithResource(resource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Update).WithResource(resource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Delete).WithResource(resource).WithAuthorize(false).Build())
            .Build();
        }
    }
}
