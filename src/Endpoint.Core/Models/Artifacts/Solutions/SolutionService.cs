using Endpoint.Core.Abstractions;
using System.Net.Http.Headers;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class SolutionService : ISolutionService
{
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IPlantUmlParserStrategyFactory _plantUmlParserStrategyFactory;

    public SolutionService(
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IPlantUmlParserStrategyFactory plantUmlParserStrategyFactory)
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
        _plantUmlParserStrategyFactory = plantUmlParserStrategyFactory;
    }

    public void AddSolutionItem(string path)
    {
        throw new NotImplementedException();
    }

    public void Create(SolutionModel model)
    {
        _artifactGenerationStrategyFactory.CreateFor(model);
    }

    public void Create(string name, string plantUmlSourcePath, string directory)
    {
        
    }

    public SolutionModel CreateFromPlantUml(string plantUml, string name, string directory)
    {
        var model = _plantUmlParserStrategyFactory.CreateFor(plantUml, new
        {
            SolutionName = name,
            SolutionRootDirectory = directory
        });

        _artifactGenerationStrategyFactory.CreateFor(model);

        return model;
    }
}