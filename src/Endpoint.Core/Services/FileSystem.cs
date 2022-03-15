using System.IO;
using System.Linq;

namespace Endpoint.Core.Services
{
    public class FileSystem : IFileSystem
    {
        public bool Exists(string path)
            => File.Exists(path);

        public bool Exists(string[] paths)
            => paths.Any(x => Exists(x));

        public Stream OpenRead(string path)
            => File.OpenRead(path);

        public string ReadAllText(string path)
            => File.ReadAllText(path);

        public void WriteAllLines(string path, string[] contents)
        {
            Delete(path);

            File.WriteAllLines(path, contents);
        }

        public string ParentFolder(string path)
        {
            var directories = path.Split(Path.DirectorySeparatorChar);

            string parentFolderPath = string.Join($"{Path.DirectorySeparatorChar}", directories.ToList()
                .Take(directories.Length - 1));

            return parentFolderPath;
        }

        public void CreateDirectory(string directory)
        {
            System.IO.Directory.CreateDirectory(directory);
        }

        public void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void DeleteDirectory(string directory)
        {
            if(Directory.Exists(directory))
            {
                Directory.Delete(directory, true); 
            }
        }

        public string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }
    }
}
