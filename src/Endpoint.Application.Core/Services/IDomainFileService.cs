using Endpoint.SharedKernal.Models;

namespace Endpoint.Application.Services
{
    public interface IDomainFileService
    {
        void Build(Settings settings);
    }
}
