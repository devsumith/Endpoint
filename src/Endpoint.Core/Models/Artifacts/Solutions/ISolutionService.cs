using Endpoint.Core.Abstractions;

namespace Endpoint.Core.Models.Artifacts.Solutions;

//https://stackoverflow.com/questions/47637228/how-can-i-add-files-to-a-solution-folder
public interface ISolutionService
{
    void AddSolutionItem(string path);
    void Create(SolutionModel model);
}


public class SolutionService : ISolutionService
{
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public SolutionService(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
    }

    public void AddSolutionItem(string path)
    {
        throw new NotImplementedException();
    }

    public void Create(SolutionModel model)
    {
        _artifactGenerationStrategyFactory.CreateFor(model);
    }
}