using Endpoint.Application.Services;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Application.Builders
{
    public class SettingsBuilder
    {
        private readonly string _domainNamespace;
        private readonly string _applicationNamespace;
        private readonly string _infrastructureNamespace;
        private readonly string _apiNamespace;
        private readonly string _domainDirectory;
        private readonly string _applicationDirectory;
        private readonly string _infrastructureDirectory;
        private readonly string _apiDirectory;
        private readonly IFileSystem _fileSystem;
        private readonly string _directory;
        private readonly string _dbContext;
        public SettingsBuilder(
            string domainNamespace,
            string applicationNamespace,
            string infrastructureNamespace,
            string apiNamespace,
            string domainDirectory,
            string applicationDirectory,
            string infrastructureDirectory,
            string apiDirectory,
            string directory,
            string dbContext,
            IFileSystem fileSystem
            )
        {
            _domainNamespace = domainNamespace;
            _applicationNamespace = applicationNamespace;
            _infrastructureNamespace = infrastructureNamespace;
            _apiNamespace = apiNamespace;
            _domainDirectory = domainDirectory;
            _applicationDirectory = applicationDirectory;
            _infrastructureDirectory = infrastructureDirectory;
            _apiDirectory = apiDirectory;
            _directory = directory;
            _fileSystem = fileSystem;
            _dbContext = dbContext;

        }

        public void Build()
        {
            var json = Serialize(new
            {
                DomainNamespace = _domainNamespace,
                ApplicationNamespace = _applicationNamespace,
                InfrastructureNamespace = _infrastructureNamespace,
                ApiNamespace = _apiNamespace,
                DomainDirectory = _domainDirectory,
                ApplicationDirectory = _applicationDirectory,
                InfrastructureDirectory = _infrastructureDirectory,
                ApiDirectory = _apiDirectory,
                Path = _directory,
                DbContext = _dbContext,
                SourceFolder = "src"
            }, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true               
            });

            _fileSystem.WriteAllLines($"{_directory}{Path.DirectorySeparatorChar}clisettings.json", new List<string> { json }.ToArray());

        }
    }
}
