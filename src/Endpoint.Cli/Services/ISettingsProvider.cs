using Endpoint.Cli.Models;

namespace Endpoint.Cli.Services
{
    public interface ISettingsProvider
    {
        CliSettings Get(string path = null);
    }
}