using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Services;

public interface ISettingsProvider
{
    SettingsModel Get(string directory = null);
}
