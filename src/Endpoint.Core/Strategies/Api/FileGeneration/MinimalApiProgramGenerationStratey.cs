using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Application;
using Endpoint.Core.Strategies.CodeBlocks;
using Endpoint.Core.Strategies.Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Strategies.Api.FileGeneration
{
    public interface IMinimalApiProgramGenerationStratey
    {
        void Create(MinimalApiProgramModel model, string directory);
    }

    public class MinimalApiProgramGenerationStratey : IMinimalApiProgramGenerationStratey
    {
        private readonly IFileSystem _fileSystem;
        private readonly IWebApplicationBuilderGenerationStrategy _webApplicationBuilderGenerationStrategy;
        private readonly IWebApplicationGenerationStrategy _webApplicationGenerationStrategy;
        private readonly ICodeBlockGenerationStrategyFactory _codeBlockGenerationStrategyFactory;
        public MinimalApiProgramGenerationStratey(
            IFileSystem fileSystem, 
            IWebApplicationGenerationStrategy webApplicationGenerationStrategy, 
            IWebApplicationBuilderGenerationStrategy webApplicationBuilderGenerationStrategy,
            ICodeBlockGenerationStrategyFactory codeBlockGenerationStrategyFactory
            )
        {
            _fileSystem = fileSystem;
            _webApplicationBuilderGenerationStrategy = webApplicationBuilderGenerationStrategy;
            _webApplicationGenerationStrategy = webApplicationGenerationStrategy;
            _codeBlockGenerationStrategyFactory = codeBlockGenerationStrategyFactory;
        }

        public void Create(MinimalApiProgramModel model, string directory)
        {
            var content = new List<string>();

            foreach(var @using in model.Usings)
            {
                content.Add($"using {@using};");
            }

            content.Add("");

            content.AddRange(_webApplicationBuilderGenerationStrategy.Create(default,default));
            
            content.Add("");

            content.AddRange(_webApplicationGenerationStrategy.Create(default, default,default));

            content.Add("");

            foreach (var entity in model.Entities)
            {
                content.Add(_codeBlockGenerationStrategyFactory.CreateFor(entity));

                content.Add("");
            }

            content.AddRange(new DbContextGenerationStrategy().Create(new DbContextModel(model.DbContextName, model.Entities)));

            _fileSystem.WriteAllLines($"{directory}{Path.DirectorySeparatorChar}Program.cs", content.ToArray());
        }
    }
}
