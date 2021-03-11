using Endpoint.Cli.Models;
using System;
using System.IO;
using System.Linq;

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
                    Console.WriteLine("Found");
                }
                i++;
            }

            return null;
        }
    }
}
