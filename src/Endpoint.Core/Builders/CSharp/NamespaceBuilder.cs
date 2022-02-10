
using Endpoint.SharedKernal.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Endpoint.SharedKernal;

namespace Endpoint.Core.Builders
{
    public class NamespaceBuilder
    {
        private List<string> _usings;
        private string _namespace;
        private string _name;
        private List<string[]> _classes;
        private IContext _context;
        private IFileSystem _fileSystem;
        private string _directory;

        public NamespaceBuilder(string @namespace, string name, IContext context, IFileSystem fileSystem)
        {
            _context = context;
            _fileSystem = fileSystem;
            _namespace = @namespace;
            _name = name;
            _usings = new List<string>();
            _classes = new List<string[]>();
        }

        public NamespaceBuilder(string name, IContext context, IFileSystem fileSystem)
        {
            _context = context;
            _fileSystem = fileSystem;
            _name = name;
            _usings = new List<string>();
            _classes = new List<string[]>();
        }

        public NamespaceBuilder WithNamespace(string @namespace)
        {
            _namespace = @namespace;
            return this;
        }

        public NamespaceBuilder WithUsing(string @using)
        {
            _usings.Add(@using);
            return this;
        }

        public NamespaceBuilder WithDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public NamespaceBuilder WithClass(string[] @class)
        {
            _classes.Add(@class);
            return this;
        }

        public void Build()
        {
            var content = new List<string>();

            if (_usings.Count > 0)
            {
                foreach (var u in _usings.OrderBy(x => x))
                {
                    content.Add($"using {u};");
                }

                content.Add("");
            }

            content.Add($"namespace {_namespace}");
            content.Add("{");
            content.Add("");

            if (_classes.Count > 0)
            {
                foreach (var @class in _classes)
                {
                    foreach (var line in @class)
                    {
                        content.Add(line.Indent(1));
                    }
                }

                content.Add("");
            }
            content.Add("}");

            var path = $"{_directory}{Path.DirectorySeparatorChar}{_name}.cs";

            _context.Add(path, content.ToArray());

            _fileSystem?.WriteAllLines(path, content.ToArray());
        }
    }
}
