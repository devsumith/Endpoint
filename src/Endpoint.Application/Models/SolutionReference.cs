using Endpoint.Application.Services;
using System.IO;

namespace Endpoint.Application.Models
{
    public class SolutionReference
    {
        private readonly string _slnDirectory;
        private readonly ICommandService _commandService;
        private readonly string _name;
        public string SrcDirectory => $"{_slnDirectory}{Path.DirectorySeparatorChar}src";
        public SolutionReference(ICommandService commandService, string slnDirectory, string name)
        {
            _commandService = commandService;
            _slnDirectory = slnDirectory;
            _name = name;
        }

        public void Add(string projectFullPath)
        {
            _commandService.Start($"dotnet sln add {projectFullPath}", _slnDirectory);
        }

        public void OpenInVisualStudio()
        {
            _commandService.Start($"start {_name}.sln", _slnDirectory);
        }
    }
}
