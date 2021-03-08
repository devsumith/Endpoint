using Microsoft.Extensions.Configuration;
using Endpoint.Cli.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using static System.IO.Path;
using static System.String;

namespace Endpoint.Cli.Services
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly IFileSystem _fileSystem;
        private readonly IConfiguration _configuration;

        public SettingsProvider(IFileSystem fileSystem, IConfiguration configuration)
        {
            _fileSystem = fileSystem;
            _configuration = configuration;
        }
        public CliSettings Get(string path = null)
        {
            path = path ?? Environment.CurrentDirectory;

            if (path.Split(DirectorySeparatorChar).Count() == 1)
                return null;

            var settingFileNames = _configuration["settingFileNames"].Split(',');

            var fullPaths = settingFileNames.Select(x => $@"{path}\{x}Settings.json").ToArray();

            if (_fileSystem.Exists(fullPaths))
            {
                var fullPath = fullPaths.First(x => _fileSystem.Exists(x));

                using (var stream = new StreamReader(fullPath))
                {
                    string json = stream.ReadToEnd();
                    var settings = JsonConvert.DeserializeObject<CliSettings>(json);
                    settings.Path = path;
                    return settings;
                }
            }
            else
            {
                return Get(ParentFolder(path));
            }
        }

        public string ParentFolder(string path)
        {
            var directories = path.Split(DirectorySeparatorChar);

            string parentFolderPath = Join($"{DirectorySeparatorChar}", directories.ToList()
                .Take(directories.Length - 1));

            return parentFolderPath;
        }
    }
}
