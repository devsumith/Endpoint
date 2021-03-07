using System;
using System.Diagnostics;

namespace Endpoint.Cli
{
    public interface ICommandService
    {
        void Start(string command, string workingDirectory = null, bool waitForExit = true);
    }
    public class CommandService : ICommandService

    {
        public void Start(string arguments, string workingDirectory = null, bool waitForExit = true)
        {
            try
            {
                workingDirectory = workingDirectory ?? Environment.CurrentDirectory;

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
            } catch(Exception e)
            {
                throw e;

            }

        }
    }
}
