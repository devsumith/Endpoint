using CommandLine;
using Endpoint.Core.Builders;
using Endpoint.Core.Enums;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands
{
    internal class Controller
    {
        [Verb("controller")]
        internal class Request : IRequest<Unit>
        {
            [Value(0)]
            public string Entity { get; set; }

            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ISettingsProvider _settingsProvder;
            private readonly IFileSystem _fileSystem;

            public Handler(ISettingsProvider settingsProvider, IFileSystem fileSystem)
            {
                _settingsProvder = settingsProvider;
                _fileSystem = fileSystem;
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvder.Get(request.Directory);

                new ClassBuilder($"{((Token)request.Entity).PascalCase}Controller", new Endpoint.SharedKernal.Services.Context(), _fileSystem)
                    .WithDirectory($"{settings.ApiDirectory}{Path.DirectorySeparatorChar}Controllers")
                    .WithUsing("System.Net")
                    .WithUsing("System.Threading.Tasks")
                    .WithUsing($"{settings.ApplicationNamespace}.Features")
                    .WithUsing("MediatR")
                    .WithUsing("Microsoft.AspNetCore.Mvc")
                    .WithNamespace($"{settings.ApiNamespace}.Controllers")
                    .WithAttribute(new AttributeBuilder().WithName("ApiController").Build())
                    .WithAttribute(new AttributeBuilder().WithName("Route").WithParam("\"api/[controller]\"").Build())
                    .WithDependency("IMediator", "mediator")
                    .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.GetById).WithResource(request.Entity).WithAuthorize(false).Build())
                    .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Get).WithResource(request.Entity).WithAuthorize(false).Build())
                    .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Create).WithResource(request.Entity).WithAuthorize(false).Build())
                    .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Page).WithResource(request.Entity).WithAuthorize(false).Build())
                    .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Update).WithResource(request.Entity).WithAuthorize(false).Build())
                    .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Delete).WithResource(request.Entity).WithAuthorize(false).Build())
                    .Build();

                return Task.FromResult(new Unit());
            }
        }
    }
}
