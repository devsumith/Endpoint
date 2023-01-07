using Endpoint.Core.Options;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public interface ISolutionModelFactory
{
    SolutionModel Create(string name);
    SolutionModel SingleProjectSolution(string name, string projectName, string dotNetProjectTypeName, string directory);
    SolutionModel Minimal(CreateEndpointSolutionOptions options);
}