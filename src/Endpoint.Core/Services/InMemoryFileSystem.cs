using System;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Services
{
    public class InMemoryFileSystem: IFileSystem
    {
        private readonly IDictionary<string, FileSystemEntry> files = new Dictionary<string, FileSystemEntry>();

        public void CreateDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public void Delete(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string path)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string[] paths)
        {
            throw new NotImplementedException();
        }

        public Stream OpenRead(string path)
        {
            throw new NotImplementedException();
        }

        public string ParentFolder(string path)
        {
            throw new NotImplementedException();
        }

        public string[] ReadAllLines(string path)
        {
            throw new NotImplementedException();
        }

        public string ReadAllText(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteAllLines(string path, string[] contents)
        {
            throw new NotImplementedException();
        }
    }

    public class FileSystemEntry
    {
        public FileAttributes Attributes { get; set; }
        public DateTimeOffset CreationTimeUtc { get; set; }
        public string Directory { get; set; }
        public string FileName { get; set; }
        public bool IsDirectory { get; set; }
        public bool IsHidden { get; set; }
        public DateTimeOffset LastAccessTimeUtc { get; set; }
        public DateTimeOffset LastWriteTimeUtc { get; set; }
        public long Length { get; set; }
        public string OriginalRootDirectory { get; set; }
        public string RootDirectory { get; set; }
    }
}
