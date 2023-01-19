using Endpoint.Core.Models.Syntax.Entities;

namespace Endpoint.Core.Models.Artifacts.Projects.Services;

public interface IApiProjectService
{
    void ControllerAdd(EntityModel entity, string directory);

}

