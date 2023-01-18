using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Methods;

namespace Endpoint.Core.Models.Syntax.Controllers;

public interface IControllerMedthodModelFactory
{
    
    MethodModel Create(ClassModel entity, RouteType routeType);
}
