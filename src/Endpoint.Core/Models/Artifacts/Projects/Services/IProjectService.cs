namespace Endpoint.Core.Models.Artifacts.Projects.Services;

public interface IProjectService
{
    void AddProject(ProjectModel model);
    void AddToSolution(ProjectModel model);
    void AddGenerateDocumentationFile(string csprojFilePath);
    void AddEndpointPostBuildTargetElement(string csprojFilePath);
    void PackageAdd(string name, string directory);
    void CoreFilesAdd(string directory);
}
