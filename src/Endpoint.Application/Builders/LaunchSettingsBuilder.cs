using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System;

namespace Endpoint.Application.Builders
{
    public class LaunchSettingsBuilder : BuilderBase<LaunchSettingsBuilder>
    {
        public LaunchSettingsBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        private Token _port = (Token)$"{5000}";
        private Token _sslPort = (Token)$"{5001}";
        private Token _projectName = (Token)"Project";
        private Token _launchUrl = (Token)$"";

        public LaunchSettingsBuilder WithPort(int port)
        {
            _port = (Token)$"{port}";
            return this;
        }

        public LaunchSettingsBuilder WithSslPort(int sslPort)
        {
            _sslPort = (Token)$"{sslPort}";
            return this;
        }

        public LaunchSettingsBuilder WithProjectName(string projectName)
        {
            _projectName = (Token)projectName;
            return this;
        }

        public LaunchSettingsBuilder WithLaunchUrl(string launchUrl)
        {
            _launchUrl = (Token)launchUrl;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(LaunchSettingsBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .With(nameof(_port), _port)
                .With(nameof(_sslPort), _sslPort)
                .With(nameof(_projectName), _projectName)
                .With(nameof(_launchUrl), _launchUrl)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/launchSettings.json", contents);

        }
    }
}
