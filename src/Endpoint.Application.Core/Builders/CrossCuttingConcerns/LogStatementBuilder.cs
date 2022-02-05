using Endpoint.Application.Enums;
using Endpoint.SharedKernal;
using Endpoint.SharedKernal.ValueObjects;

namespace Endpoint.Application.Builders
{
    internal class LogStatementBuilder
    {

        private int _indent;
        private string _resource;
        private EndpointType _endpointType;

        public LogStatementBuilder(string resource, EndpointType? endpointType = EndpointType.Create, int indent = 0)
        {
            _indent = indent;
            _resource = resource ?? throw new System.ArgumentNullException(nameof(resource));
            _endpointType = endpointType ?? throw new System.ArgumentNullException(nameof(endpointType));
        }

        public string[] BuildForCreateCommand()
            => new string[4]
            {
                "_logger.LogInformation(".Indent(_indent),
                "\"----- Sending command: {CommandName}: ({@Command})\",".Indent(_indent + 1),
                $"nameof(Create{((Token)_resource).PascalCase}Request),".Indent(_indent + 1),
                "request);".Indent(_indent + 1)
            };

        public string[] BuildForUpdateCommand()
            => new string[6]
            {
                "_logger.LogInformation(".Indent(_indent),
                "\"----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})\",".Indent(_indent + 1),
                $"nameof(Update{((Token)_resource).PascalCase}Request),".Indent(_indent + 1),
                $"nameof(request.{((Token)_resource).PascalCase}.{((Token)_resource).PascalCase}Id),".Indent(_indent + 1),
                $"request.{((Token)_resource).PascalCase}.{((Token)_resource).PascalCase}Id,".Indent(_indent + 1),
                "request);".Indent(_indent + 1)
            };

        public string[] BuildForDeleteCommand()
            => new string[6]
            {
                "_logger.LogInformation(".Indent(_indent),
                "\"----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})\",".Indent(_indent + 1),
                $"nameof(Remove{((Token)_resource).PascalCase}Request),".Indent(_indent + 1),
                $"nameof(request.{((Token)_resource).PascalCase}Id),".Indent(_indent + 1),
                $"request.{((Token)_resource).PascalCase}Id,".Indent(_indent + 1),
                "request);".Indent(_indent + 1)
            };

        public string[] Build()
        {
            switch (_endpointType)
            {
                case EndpointType.Create:
                    return BuildForCreateCommand();


                case EndpointType.Update:
                    return BuildForUpdateCommand();


                case EndpointType.Delete:
                    return BuildForDeleteCommand();
            }

            return new string[0] { };
        }
    }
}
