using Endpoint.Core.Models.Artifacts;
using Endpoint.Core.Services;
using System.Linq;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public class SolutionUpdateStrategy : ISolutionUpdateStrategy
    {
        private readonly ICommandService _commandService;

        public SolutionUpdateStrategy(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public bool CanHandle(SolutionModel previous, SolutionModel next) => true;
        public int Order { get; set; }
        public void Update(SolutionModel previous, SolutionModel next)
        {
            foreach (var project in next.Projects.Where(np => previous.Projects.SingleOrDefault(p => p.Path == np.Path) == null))
            {
                if (previous.Projects.SingleOrDefault(x => x.Name == project.Name) == null)
                {
                    _commandService.Start($"dotnet sln {next.SolultionFileName} add {project.Path}", next.SolutionDirectory);
                }
            }
        }
    }
}
