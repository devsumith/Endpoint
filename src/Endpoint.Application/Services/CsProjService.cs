using System.Linq;
using System.Xml.Linq;

namespace Endpoint.Application.Services
{
    public interface ICsProjService
    {
        void AddGenerateDocumentationFile(string csprojFilePath);
        void AddEndpointPostBuildTargetElement(string csprojFilePath);
    }

    public class CsProjService: ICsProjService
    {
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

        public void AddEndpointPostBuildTargetElement(string csprojFilePath)
        {
            var doc = XDocument.Load(csprojFilePath);
            var projectNode = doc.FirstNode as XElement;
            projectNode.Add(_getEndpointPostBuildTargetElement());
            doc.Save(csprojFilePath);
        }

        private XElement _getEndpointPostBuildTargetElement()
        {
            var dotnetToolRestoreCommand = new XElement("Exec");
            
            dotnetToolRestoreCommand.SetAttributeValue("Command", "dotnet tool restore");

            var toFileCommand = new XElement("Exec");
            
            toFileCommand.SetAttributeValue("Command", "dotnet tool run swagger tofile --serializeasv2  --output \"$(ProjectDir)swagger.json\" \"$(TargetDir)$(TargetFileName)\" v1");

            var endpointCommand = new XElement("Exec");
            
            endpointCommand.SetAttributeValue("Command", "endpoint post-api-build");
            
            var element = new XElement("Target",dotnetToolRestoreCommand, toFileCommand, endpointCommand);
            
            element.SetAttributeValue("Name", "EndpointPostBuildTarget");
            
            element.SetAttributeValue("AfterTargets", "Build");
            
            return element;
        }
    }
}
