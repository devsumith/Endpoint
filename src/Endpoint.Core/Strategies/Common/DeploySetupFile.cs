using Endpoint.Core.Models.Options;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.IO;

namespace Endpoint.Core.Strategies.Common
{

    public interface IDeploySetupFileGenerationStrategy
    {
        void Generate(SettingsModel settings);
    }

    public class DeploySetupFileGenerationStrategy : IDeploySetupFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;

        public DeploySetupFileGenerationStrategy(IFileSystem fileSystem, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor)
        {
            _fileSystem = fileSystem;
            _templateLocator = templateLocator;
            _templateProcessor = templateProcessor;
        }
        public void Generate(SettingsModel settings)
        {
            var tokens = new TokensBuilder()
                .With("Name", (Token)settings.SolutionName)
                .With("ApiProjectName",(Token)$"{settings.SolutionName}.Api")
                .Build();

            var template = _templateLocator.Get("DeploySetupFile");

            var result = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($"{settings.RootDirectory}{Path.DirectorySeparatorChar}deploy{Path.DirectorySeparatorChar}setup.ps1", result);

        }
    }
}
