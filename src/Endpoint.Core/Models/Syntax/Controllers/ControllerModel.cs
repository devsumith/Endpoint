using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Classes;

namespace Endpoint.Core.Models.Syntax.Controllers;

public class ControllerModel: ClassModel
{
    public ControllerModel(string name)
        :base(name) { }
}

public class ControllerGenerationStrategy: SyntaxGenerationStrategyBase<ControllerModel>
{        
    public ControllerGenerationStrategy(IServiceProvider serviceProvider)
        :base(serviceProvider)
    { }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, ControllerModel model, dynamic configuration = null)
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
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.GetById).WithResource(model).WithAuthorize(false).Build())
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.Get).WithResource(model).WithAuthorize(false).Build())
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.Create).WithResource(model).WithAuthorize(false).Build())
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.Page).WithResource(model).WithAuthorize(false).Build())
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.Update).WithResource(model).WithAuthorize(false).Build())
        .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(EndpointType.Delete).WithResource(model).WithAuthorize(false).Build())
        .Build();
*/
        return "";
    }


}
