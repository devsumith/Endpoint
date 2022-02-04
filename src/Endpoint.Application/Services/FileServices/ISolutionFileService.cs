namespace Endpoint.Application.Services.FileServices
{
    public interface ISolutionFileService
    {
        public Models.Settings Build(string name, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, string resource, string directory, bool isMicroserviceArchitecture);
    }
}
