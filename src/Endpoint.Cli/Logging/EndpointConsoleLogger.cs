using Microsoft.Extensions.Logging;
using System;

namespace Endpoint.Cli.Logging
{
    public class EndpointConsoleLogger : ILogger
    {
        private readonly EndpointLoggerOptions _options;

        public EndpointConsoleLogger(EndpointLoggerOptions options)
        {
            _options = options;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = formatter(state, exception);

            if (_options.EnableColors)
            {
                LogMessageWithColors(logLevel, message);
            }
            else
            {
                LogMessage(message);
            }
        }

        private void LogMessageWithColors(LogLevel logLevel, string message)
        {
            var color = logLevel switch
            {
                LogLevel.Critical => _options.ErrorColor,
                LogLevel.Error => _options.ErrorColor,
                LogLevel.Warning => _options.WarningColor,
                _ => Console.ForegroundColor
            };

            ConsoleColor saved = Console.ForegroundColor;
            Console.ForegroundColor = color;

            LogMessage(message);

            Console.ForegroundColor = saved;
        }

        private void LogMessage(string message)
        {
            _options.Writer.WriteLine(message);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                case LogLevel.Critical:
                case LogLevel.Information:
                case LogLevel.Warning:
                    return true;

                default:
                    return false;
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NullDisposable();
        }

        private class NullDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}