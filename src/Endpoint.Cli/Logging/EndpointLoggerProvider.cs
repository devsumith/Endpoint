using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Logging
{
    public class EndpointLoggerProvider : ILoggerProvider
    {
        private readonly EndpointLoggerOptions options;

        public EndpointLoggerProvider(EndpointLoggerOptions options)
        {
            this.options = options;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EndpointConsoleLogger(this.options);
        }
    }
}
