namespace Endpoint.Core.Models.WebArtifacts.Services;

public interface IAngularService
{
    void CreateWorkspace(string name, string projectName, string projectType, string prefix, string rootDirectory);
    void AddProject(AngularProjectModel model);
    void EnableDefaultStandaloneComponents(string directory, string projectName);
    void KarmaRemove(string directory); 
    void UpdateCompilerOptionsToUseJestTypes(AngularProjectModel model);
    void NgxTranslateAdd(string projectName, string directory);

}

