namespace Endpoint.Core.Services
{
    public interface ICsProjectService
    {
        void AddGenerateDocumentationFile(string csprojFilePath);
        void AddEndpointPostBuildTargetElement(string csprojFilePath);
    }
}
