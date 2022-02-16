using Endpoint.Core.Exceptions;
using Endpoint.Core.Models;
using System.IO;
using System.Linq;
using static System.Environment;
using static System.IO.Path;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Core.Services
{
    public interface ISettingsProvider
    {
        Settings Get(string directory = null);
    }
    public class SettingsProvider : ISettingsProvider
    {
        public Settings Get(string directory = null)
        {
            directory ??= CurrentDirectory;

            var parts = directory.Split(DirectorySeparatorChar);

            for (var i = 1; i <= parts.Length; i++)
            {
                var path = $"{string.Join(DirectorySeparatorChar, parts.Take(i))}{DirectorySeparatorChar}{CoreConstants.SettingsFileName}";

                if (File.Exists(path))
                {
                    var settings = Deserialize<Settings>(File.ReadAllText(path), new()
                    {
                        PropertyNameCaseInsensitive = true,
                    });
                    settings.RootDirectory = new FileInfo(path).Directory.FullName;
                    return settings;
                }

                i++;
            }

            return null;

        }
    }
}
