using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Files.Create;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Endpoint.Core.Strategies
{
    public interface IProjectGenerationStrategy
    {
        void Create(ProjectModel model);
    }

    public class ProjectGenerationStrategy: IProjectGenerationStrategy
    {
        private readonly IFileGenerationStrategyFactory _fileGenerationStrategyFactory;
        private readonly ICommandService _commandService;
        private readonly IFileSystem _fileSystem;

        public ProjectGenerationStrategy(
            IFileSystem fileSystem,
            ICommandService commandService,
            IFileGenerationStrategyFactory fileGenerationStrategyFactory
            )
        {
            _commandService = commandService;
            _fileGenerationStrategyFactory = fileGenerationStrategyFactory;
            _fileSystem = fileSystem;
        }

        public void Create(ProjectModel model)
        {
            foreach(var path in _fileSystem.GetFiles(model.Directory,"*.cs", SearchOption.AllDirectories))
            {
                _fileSystem.Delete(path);
            }

            foreach(var package in model.Packages)
            {
                var version = package.IsPreRelease ? "--prerelease" : $"--version {package.Version}";

                _commandService.Start($"dotnet add package {package.Name} {version}",model.Directory);
            }

            foreach(var file in model.Files)
            {
                _fileGenerationStrategyFactory.CreateFor(file);
            }

            if(model.GenerateDocumentationFile)
            {
                var doc = XDocument.Load(model.Path);
                var projectNode = doc.FirstNode as XElement;

                var element = projectNode.Nodes()
                    .Where(x => x.NodeType == System.Xml.XmlNodeType.Element)
                    .First(x => (x as XElement).Name == "PropertyGroup") as XElement;

                element.Add(new XElement("GenerateDocumentationFile", true));
                element.Add(new XElement("NoWarn", "$(NoWarn);1591"));
                doc.Save(model.Path);
            }
        }

    }
}
