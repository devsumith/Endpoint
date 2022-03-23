using Endpoint.Core.Models;

namespace Endpoint.Core.Services
{
    public interface ISettingsProvider
    {
        Settings Get(string directory = null);
    }
}
