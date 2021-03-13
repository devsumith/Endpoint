using Endpoint.Application.Services;

namespace Endpoint.Application.Models
{
    public class ProjectReference
    {
        private readonly ICommandService _commandService;
        private readonly string _projectDirectory;
        private string _namespace;

        public string FullPath { get; set; }
        public string Directory => _projectDirectory;
        public string Namespace => _namespace;
        public ProjectReference(ICommandService commandService, string projectDirectory, string fullPath, string @namespace)
        {
            _commandService = commandService;
            _projectDirectory = projectDirectory;
            FullPath = fullPath;
            _namespace = @namespace;
        }

        public void Run()
        {
            _commandService.Start("dotnet run watch", _projectDirectory);
        }
    }
}
