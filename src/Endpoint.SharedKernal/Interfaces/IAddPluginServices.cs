using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.SharedKernal.Interfaces
{
    public interface IAddPluginServices
    {
        void Add(IServiceCollection services);
    }
}
