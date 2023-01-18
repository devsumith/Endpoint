using Endpoint.Core.Models.Syntax.Classes;

namespace Endpoint.Core.Models.Syntax.Controllers;

public interface IControllerModelFactory
{
    ClassModel Create(ClassModel entity);
}
