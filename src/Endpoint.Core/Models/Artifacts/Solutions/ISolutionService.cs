namespace Endpoint.Core.Models.Artifacts.Solutions;

//https://stackoverflow.com/questions/47637228/how-can-i-add-files-to-a-solution-folder
public interface ISolutionService
{
    void AddSolutionItem(string path);
    void Create(SolutionModel model);
    void Create(string name, string plantUmlSourcePath, string directory);
    SolutionModel CreateFromPlantUml(string plantUml, string name, string directory);
}
