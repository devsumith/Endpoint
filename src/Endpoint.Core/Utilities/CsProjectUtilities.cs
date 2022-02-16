using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Endpoint.Core.Utilities
{
    public static class CsProjectUtilities
    {
        public static void RemoveDefaultFiles(string directory)
        {
            _removeDefaultFile($"{directory}{Path.DirectorySeparatorChar}Class1.cs");
        }

        public static void RemoveDefaultWebApiFiles(string directory)
        {
            _removeDefaultFile($"{directory}{Path.DirectorySeparatorChar}WeatherForecast.cs");
            _removeDefaultFile($"{directory}{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}WeatherForecastController.cs");

        }
        private static void _removeDefaultFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void AddGenerateDocumentationFile(string csprojFilePath)
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

        private static void _addEndpointPostBuildTargetElement(string csprojFilePath)
        {
            var doc = XDocument.Load(csprojFilePath);
            var projectNode = doc.FirstNode as XElement;
            projectNode.Add(_createEndpointPostBuildTargetElement());
            doc.Save(csprojFilePath);
        }

        private static XElement _createEndpointPostBuildTargetElement()
        {
            var dotnetToolRestoreCommand = new XElement("Exec");

            dotnetToolRestoreCommand.SetAttributeValue("Command", "dotnet tool restore");

            var toFileCommand = new XElement("Exec");

            toFileCommand.SetAttributeValue("Command", "dotnet tool run swagger tofile --serializeasv2  --output \"$(ProjectDir)swagger.json\" \"$(TargetDir)$(TargetFileName)\" v1");

            var endpointCommand = new XElement("Exec");

            endpointCommand.SetAttributeValue("Command", "endpoint post-api-build");

            var element = new XElement("Target", dotnetToolRestoreCommand, toFileCommand, endpointCommand);

            element.SetAttributeValue("Name", "EndpointPostBuildTarget");

            element.SetAttributeValue("AfterTargets", "Build");

            return element;
        }
    }
}
