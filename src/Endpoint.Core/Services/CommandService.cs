using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Endpoint.Core.Services
{
    public class CommandService : ICommandService
    {
        private readonly ILogger _logger;
        public CommandService(
            ILogger logger
            
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Start(string arguments, string workingDirectory = null, bool waitForExit = true)
        {
            // Detect if on Mac and do this?
            // https://stackoverflow.com/questions/28487132/running-bash-commands-from-c-sharp

            try
            {
                workingDirectory ??= Environment.CurrentDirectory;

                _logger.LogInformation($"{arguments} in {workingDirectory}");

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = "cmd.exe",
                        Arguments = $"/C {arguments}",
                        WorkingDirectory = workingDirectory
                    }
                };

                process.Start();

                if (waitForExit)
                {
                    process.WaitForExit();
                }
            }
            catch
            {
                throw;
            }

        }
    }
}
