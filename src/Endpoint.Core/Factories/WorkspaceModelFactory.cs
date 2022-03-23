using Endpoint.Core.Models;
using Endpoint.Core.Options;

namespace Endpoint.Core.Factories
{
    public static class WorkspaceModelFactory
    {
        public static WorkspaceModel CreateMinimalApiEndpointWorkspace(CreateEndpointOptions request)
        {
            var workspaceModel = new WorkspaceModel
            {
                Name = request.Name,
                ParentDirectory = request.Directory,
            };


            workspaceModel.Solutions.Add(SolutionModelFactory.Minimal(new ()
            {
                Name=request.Name,
                Port=request.Port,
                Properties=request.Properties,
                Resource = request.Resource,
                Monolith = request.Monolith,
                Minimal = request.Minimal,
                DbContextName = request.DbContextName,
                ShortIdPropertyName = request.ShortIdPropertyName,
                NumericIdPropertyDataType = request.NumericIdPropertyDataType,
                Directory = request.Directory
            }));

            return workspaceModel;
        }
    }
}
