namespace Endpoint.Application.Services.FileServices
{
    public interface IApplicationFileService
    {
        void Build(Models.Settings settings);
        void BuildAdditionalResource(string additionalResource, Models.Settings settings);
    }
}
