namespace Endpoint.Application.Services.FileServices
{
    /*1. SolutionFileService -> Create All Projects and Files in Solution

    2. DomainFileService -> Create All Files and Folders in Domain

    3. ApplicationFileService -> Create All Files and Folders in Application

    4. InfrastructureFileService -> Create All Files and Folders in Infrastructure

    5. ApiFileService -> Create All Files and Folders in Api

    6. TestingFileService -> Create All Files and Folders in Testing

    7. UnitTestsFileService -> Create All Files and Folders in UnitTests

    8. IntegrationTestsFileService -> Create All Files and Folders in UnitTests*/

    public interface ISolutionFileService
    {
        public Models.Settings Build(string name, string resource, string directory, bool isMicroserviceArchitecture);
    }
}
