using Endpoint.Core.Models.Artifacts.Solutions;

namespace Endpoint.Core.Models.Syntax;

public interface ISyntaxService
{
    SyntaxModel SyntaxModel { get; set; }
    SolutionModel SolutionModel { get; set; }
}
