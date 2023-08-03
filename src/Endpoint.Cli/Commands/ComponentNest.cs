// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Endpoint.Cli.Commands;


[Verb("component-nest")]
public class ComponentNestRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ComponentNestRequestHandler : IRequestHandler<ComponentNestRequest>
{
    private readonly ILogger<ComponentNestRequestHandler> _logger;

    public ComponentNestRequestHandler(ILogger<ComponentNestRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ComponentNestRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ComponentNestRequestHandler));

        foreach (var file in Directory.GetFiles(request.Directory, "*component.ts", SearchOption.AllDirectories))
        {
            var componentName = Path.GetFileNameWithoutExtension(file).Split('.')[0];

            var directory = Path.GetDirectoryName(file);

            if (!directory.EndsWith(componentName))
            {
                var destinationDirectory = Path.Combine(directory, componentName);

                Directory.CreateDirectory(destinationDirectory);

                foreach (var fileExtension in new[] { ".ts", ".scss", ".html" })
                {
                    var source = Path.Combine(directory, $"{componentName}.component.{fileExtension}");

                    var destination = Path.Combine(destinationDirectory, $"{componentName}.component.{fileExtension}");

                    if (File.Exists(source))
                        File.Move(source, destination);
                }
            }
        }
    }
}
