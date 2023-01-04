using Endpoint.Core.Models.Artifacts;
using Endpoint.Core.Options;
using System;

namespace Endpoint.Core.Factories
{
    [Obsolete]
    public static class _WorkspaceSettingsModelFactory
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
