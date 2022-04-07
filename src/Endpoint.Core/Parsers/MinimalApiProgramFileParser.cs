using Endpoint.Core.Models;

namespace Endpoint.Core.Parsers
{
    public static class MinimalApiProgramFileParser
    {
        public static MinimalApiProgramFileModel Parse(string path, IMinimalApiProgramFileParserStrategyFactory factory)
        {
            return factory.ParseFor(path);
        }
    }

    public interface IMinimalApiProgramFileParserStrategyFactory
    {
        MinimalApiProgramFileModel ParseFor(string path);
    }
}
