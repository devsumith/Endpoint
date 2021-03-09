using Endpoint.Cli.Services;
using System;

namespace Endpoint.Cli.Builders
{
    public class ControllerBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;

        private string _directory;
        private string _name;

        public ControllerBuilder(IServiceProvider serviceProvider)
        {
            _commandService = serviceProvider.GetService(typeof(ICommandService)) as ICommandService;
        }

        public ControllerBuilder SetDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public ControllerBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public ControllerBuilder(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public void Build()
        {

        }
    }
}
