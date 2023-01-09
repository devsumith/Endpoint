namespace Endpoint.Core.Services;

public interface IMinimalApiService
{
    void Create(string name, string dbContextName, string entityName, string directory);
}
