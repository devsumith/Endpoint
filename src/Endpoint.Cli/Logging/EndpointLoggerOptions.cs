using System;
using System.IO;

namespace Allagi.Endpoint.Cli.Logging
{
    public class EndpointLoggerOptions
    {
        public EndpointLoggerOptions(bool enableColors, ConsoleColor errorColor, ConsoleColor warningColor, TextWriter writer)
        {
            EnableColors = enableColors;
            ErrorColor = errorColor;
            WarningColor = warningColor;
            Writer = writer;
        }

        public bool EnableColors { get; }

        public ConsoleColor ErrorColor { get; }

        public ConsoleColor WarningColor { get; }

        public TextWriter Writer { get; }
    }
}
