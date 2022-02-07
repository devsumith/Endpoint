using CommandLine;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static System.IO.Path;

namespace Endpoint.Commands
{
    internal class Plugin
    {
        [Verb("plugin")]
        internal class Request : IRequest<Unit> {
            [Value(0)]
            public string Name { get; set; }

            [Option('d', Required = false)]
            public string Directory { get; set; }
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ICommandService _commandService;
            private readonly ISettingsProvider _settingsProvider;

            public Handler(ICommandService commandService, ISettingsProvider settingsProvider)
            {
                _commandService = commandService;
                _settingsProvider = settingsProvider;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvider.Get(request.Directory);

                var pluginsDirectory = $@"{settings.RootDirectory}{DirectorySeparatorChar}{settings.SourceFolder}{DirectorySeparatorChar}Plugins";

                var pluginName = $"{settings.SolutionName}.Application.Plugins.{((Token)request.Name).PascalCase}";

                var pluginDirectory = $"{pluginsDirectory}{DirectorySeparatorChar}{pluginName}";

                var pluginCsProjectFullPath = $"{pluginDirectory}{DirectorySeparatorChar}{pluginName}.csproj";

                var cliCsProjectDirectory = $"{settings.RootDirectory}{DirectorySeparatorChar}{settings.SourceFolder}{DirectorySeparatorChar}{settings.SolutionName}.Cli";

                _commandService.Start($"dotnet new classlib -o {pluginName} --framework net5.0", pluginsDirectory);

                _removeDefaultFiles(pluginDirectory);

                _commandService.Start($"dotnet sln add {pluginCsProjectFullPath}", settings.RootDirectory);

                _commandService.Start($"dotnet add {cliCsProjectDirectory} reference {pluginCsProjectFullPath}");

                return new();
            }

            protected void _removeDefaultFiles(string directory)
            {
                _removeDefaultFile($"{directory}{Path.DirectorySeparatorChar}Class1.cs");
            }

            private void _removeDefaultFile(string path)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
