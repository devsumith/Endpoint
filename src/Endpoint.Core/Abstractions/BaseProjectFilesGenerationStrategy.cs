using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Endpoint.Core.Services
{
    public abstract class BaseProjectFilesGenerationStrategy
    {
        protected readonly ICommandService _commandService;
        protected readonly ITemplateProcessor _templateProcessor;
        protected readonly ITemplateLocator _templateLocator;
        protected readonly IFileSystem _fileSystem;

        public BaseProjectFilesGenerationStrategy(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem)
        {
            _commandService = commandService ?? throw new System.ArgumentNullException(nameof(commandService));
            _templateProcessor = templateProcessor ?? throw new System.ArgumentNullException(nameof(templateProcessor));
            _templateLocator = templateLocator ?? throw new System.ArgumentNullException(nameof(templateLocator));
            _fileSystem = fileSystem ?? throw new System.ArgumentNullException(nameof(fileSystem));
        }

        protected void _removeDefaultFiles(string directory)
        {
            _removeDefaultFile($"{directory}{Path.DirectorySeparatorChar}Class1.cs");
        }

        private void _removeDefaultFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        protected void _createFolder(string folder, string directory)
        {
            folder = folder.Replace($"{directory}{Path.DirectorySeparatorChar}", "");

            string[] parts = folder.Split(Path.DirectorySeparatorChar);

            foreach (string part in parts)
            {
                var newDirectory = $"{directory}{Path.DirectorySeparatorChar}{part}";

                if (!Directory.Exists(newDirectory))
                {
                    Directory.CreateDirectory(newDirectory);
                }

                directory = newDirectory;
            }
        }

        public void AddGenerateDocumentationFile(string csprojFilePath)
        {
            var doc = XDocument.Load(csprojFilePath);
            var projectNode = doc.FirstNode as XElement;

            var element = projectNode.Nodes()
                .Where(x => x.NodeType == System.Xml.XmlNodeType.Element)
                .First(x => (x as XElement).Name == "PropertyGroup") as XElement;

            element.Add(new XElement("GenerateDocumentationFile", true));
            element.Add(new XElement("NoWarn", "$(NoWarn);1591"));
            doc.Save(csprojFilePath);
        }

        protected void _addEndpointPostBuildTargetElement(string csprojFilePath)
        {
            var doc = XDocument.Load(csprojFilePath);
            var projectNode = doc.FirstNode as XElement;
            projectNode.Add(_createEndpointPostBuildTargetElement());
            doc.Save(csprojFilePath);
        }

        private XElement _createEndpointPostBuildTargetElement()
        {
            var dotnetToolRestoreCommand = new XElement("Exec");

            dotnetToolRestoreCommand.SetAttributeValue("Command", "dotnet tool restore");

            var toFileCommand = new XElement("Exec");

            toFileCommand.SetAttributeValue("Command", "dotnet tool run swagger tofile --serializeasv2  --output \"$(ProjectDir)swagger.json\" \"$(TargetDir)$(TargetFileName)\" v1");

            var element = new XElement("Target", dotnetToolRestoreCommand, toFileCommand);

            element.SetAttributeValue("Name", "EndpointPostBuildTarget");

            element.SetAttributeValue("AfterTargets", "Build");

            return element;
        }
    }
}
