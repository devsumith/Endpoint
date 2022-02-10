using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.Services;
using System.IO;

namespace Endpoint.Core.Models
{
    public class CsProject
    {
        public string FullPath { get; set; }
        public string Directory { get; private set; }
        public string Namespace { get; private set; }

        public static void Run(ICommandService commandService, string directory)
        {
            commandService.Start("dotnet run watch", directory);
        }

        public static void RemoveDefaultFiles(string directory)
        {
            _removeDefaultFile($"{directory}{Path.DirectorySeparatorChar}Class1.cs");
        }

        private static void _removeDefaultFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void AddProjectReference(ICommandService commandService, string directory, string referencedCsProjectFullPath)
        {
            commandService.Start($"dotnet add {directory} reference {referencedCsProjectFullPath}");
        }

        public CsProject()
        {

        }

        public CsProject(Settings settings, CsProjectType csProjectType)
        {

        }
    }
}
