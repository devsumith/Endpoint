// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Services;

public class CommandService : ICommandService
{
    private readonly ILogger<CommandService> logger;

    public CommandService(ILogger<CommandService> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Start(string arguments, string workingDirectory = null, bool waitForExit = true)
    {
        workingDirectory ??= Environment.CurrentDirectory;

        logger.LogInformation($"{arguments} in {workingDirectory}");

        var process = IsUnix() ? UnixBash(arguments, workingDirectory) : WindowsCmd(arguments, workingDirectory);

        process.Start();

        if (waitForExit)
        {
            process.WaitForExit();
        }
    }

    private bool IsUnix() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    private Process UnixBash(string arguments, string workingDirectory)
        => new ()
        {
            StartInfo = new ()
            {
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = "bash",
                Arguments = $"-c \"{arguments}\"",
                WorkingDirectory = workingDirectory,
            },
        };

    private Process WindowsCmd(string arguments, string workingDirectory)
        => new ()
        {
            StartInfo = new ()
            {
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = "cmd.exe",
                Arguments = $"/C {arguments}",
                WorkingDirectory = workingDirectory,
            },
        };
}
