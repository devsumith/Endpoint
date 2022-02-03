namespace Endpoint.Application.Services.FileServices
{
    public interface IApiFileService
    {
        void Build(Models.Settings settings);
        void BuildAdditionalResource(string additionalResource, Models.Settings settings);
    }
}
