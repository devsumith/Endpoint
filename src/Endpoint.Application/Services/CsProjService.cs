using System.Linq;
using System.Xml.Linq;

namespace Endpoint.Application.Services
{
    public interface ICsProjService
    {
        void AddGenerateDocumentationFile(string csprojFilePath);
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
    }
}
