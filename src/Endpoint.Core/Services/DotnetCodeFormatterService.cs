// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Services;

public class DotnetCodeFormatterService: ICodeFormatterService
{
    private readonly ILogger<DotnetCodeFormatterService> _logger;
    private readonly IFileProvider fileProvider;
    private readonly ICommandService commandService;
    private readonly IFileSystem fileSystem;

    public DotnetCodeFormatterService(ILogger<DotnetCodeFormatterService> logger, IFileProvider fileProvider, ICommandService commandService, IFileSystem fileSystem){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.commandService= commandService ?? throw new ArgumentNullException(nameof(commandService));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public async Task FormatAsync(string directory)
    {
        _logger.LogInformation("Formatting files using dotnet format");

        var solutionDirectory = fileSystem.Path.GetDirectoryName(fileProvider.Get("*.sln", directory));

        commandService.Start("dotnet format", solutionDirectory);

    }

}

