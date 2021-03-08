using System.IO;

namespace Endpoint.Cli.Services
{
    public interface IFileSystem
    {
        string ReadAllText(string path);
        Stream OpenRead(string path);
        bool Exists(string path);
        bool Exists(string[] paths);
        void WriteAllLines(string path, string[] contents);
        string ParentFolder(string path);
    }
}