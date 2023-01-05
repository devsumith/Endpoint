using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Strategies.Tests
{
    public interface IMinimalApiTestsFileGenerationStrategy
    {
        void Create(MinimalApiProgramModel model, string directory);
    }

    public class MinimalApiTestsFileGenerationStrategy : IMinimalApiTestsFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;
        public MinimalApiTestsFileGenerationStrategy(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new System.ArgumentNullException(nameof(fileSystem)); 
        }

        public void Create(MinimalApiProgramModel model, string directory)
        {
            var content = new List<string>();
            
            _fileSystem.WriteAllText($"{directory}{Path.DirectorySeparatorChar}Tests.cs", string.Join(Environment.NewLine, content));
        }
    }
}
