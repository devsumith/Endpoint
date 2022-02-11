using System.IO;

namespace Endpoint.Core.Services
{
    public abstract class BaseProjectFilesGenerationStrategy
    {
        protected readonly ICommandService _commandService;
        protected readonly ITemplateProcessor _templateProcessor;
        protected readonly ITemplateLocator _templateLocator;
        protected readonly IFileSystem _fileSystem;

        public BaseProjectFilesGenerationStrategy(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem)
        {
            _commandService = commandService;
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
            _fileSystem = fileSystem;
        }

        protected void _removeDefaultFiles(string directory)
        {
            _removeDefaultFile($"{directory}{Path.DirectorySeparatorChar}Class1.cs");
        }

        private void _removeDefaultFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        protected void _createFolder(string folder, string directory)
        {
            folder = folder.Replace($"{directory}{Path.DirectorySeparatorChar}", "");

            string[] parts = folder.Split(Path.DirectorySeparatorChar);

            foreach (string part in parts)
            {
                var newDirectory = $"{directory}{Path.DirectorySeparatorChar}{part}";

                if (!Directory.Exists(newDirectory))
                {
                    Directory.CreateDirectory(newDirectory);
                }

                directory = newDirectory;
            }
        }
    }
}
