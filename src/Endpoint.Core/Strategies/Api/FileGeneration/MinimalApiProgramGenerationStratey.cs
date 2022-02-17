using Endpoint.Core.Models;
using Endpoint.Core.Services;
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
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IWebApplicationBuilderGenerationStrategy _webApplicationBuilderGenerationStrategy;
        private readonly IWebApplicationGenerationStrategy _webApplicationGenerationStrategy;

        public MinimalApiProgramGenerationStratey(IFileSystem fileSystem, ITemplateProcessor templateProcessor, ITemplateLocator templateLocator)
        {
            _fileSystem = fileSystem;
            _webApplicationBuilderGenerationStrategy = new WebApplicationBuilderGenerationStrategy(_templateProcessor, templateLocator);
        }

        public void Create(MinimalApiProgramModel model, string directory)
        {
            var content = new List<string>();

            foreach(var @using in model.Usings)
            {
                content.Add($"using {@using};");
            }

            content.Add("");

            content.AddRange(_webApplicationBuilderGenerationStrategy.Create(model));
            
            content.Add("");

            content.AddRange(_webApplicationGenerationStrategy.Create(model));

            content.Add("");

            foreach (var aggregateRoot in model.AggregateRoots)
            {
                // generate a model

                content.Add("");
            }

            // dbcontext

            _fileSystem.WriteAllLines($"{directory}{Path.DirectorySeparatorChar}Program.cs", content.ToArray());
        }
    }
}
