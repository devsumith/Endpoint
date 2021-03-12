using Endpoint.Cli.Models;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Endpoint.Cli.Services
{
    public interface ISettingsProvider
    {
        Settings Get(string directory = null);
    }
    public class SettingsProvider : ISettingsProvider
    {
        public Settings Get(string directory = null)
        {
            directory ??= System.Environment.CurrentDirectory;

            var parts = directory.Split(Path.DirectorySeparatorChar);

            for(var i = 1; i <= parts.Length; i++)
            {
                var path = $"{string.Join(Path.DirectorySeparatorChar, parts.Take(i))}{Path.DirectorySeparatorChar}clisettings.json";

                if (File.Exists(path))
                {
                    return JsonSerializer.Deserialize<Settings>(File.ReadAllText(path), new () { 
                        PropertyNameCaseInsensitive = true, 
                    });
                }

                i++;
            }

            return Settings.Empty;
            
        }
    }
}
