using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class ControllerBuilder : BuilderBase<ControllerBuilder>
    {
        public ControllerBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        private Token _resource;

        public ControllerBuilder SetResource(string resource)
        {
            _resource = (Token)resource;
            return this;
        }

        public void Build()
        {
            var context = new Context();
            new ClassBuilder($"{_resource.PascalCase}Controller", context, _fileSystem)
                .WithDirectory($"{_apiDirectory.Value}{Path.DirectorySeparatorChar}Controllers")
                .WithUsing("System.Net")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("Microsoft.AspNetCore.Mvc")
                .WithUsing("MediatR")
                .WithUsing("Microsoft.Extensions.Logging")
                .WithNamespace($"{_apiNamespace.Value}.Controllers")
                .WithAttribute(new AttributeBuilder().WithName("ApiController").Build())
                .WithAttribute(new AttributeBuilder().WithName("Route").WithParam("\"api/[controller]\"").Build())
                .WithDependency("IMediator", "mediator")
                .WithDependency($"ILogger<{_resource.PascalCase}Controller>", "logger")
                .Build();
        }
    }
}
