using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.Collections.Generic;
using System.IO;
using static Endpoint.Application.Constants;

namespace Endpoint.Application.Builders
{
    public class EventBuilder : BuilderBase<EventBuilder>
    {
        public EventBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        private Token _aggregate;
        private Token _eventName;

        public EventBuilder WithAggregate(string aggregate)
        {
            _aggregate = (Token)aggregate;
            return this;
        }

        public EventBuilder WithEvent(string eventName)
        {
            _eventName = (Token)eventName;
            return this;
        }

        public void Build()
        {
            var tokens = new TokensBuilder()
                .With(nameof(_eventName), _eventName)
                .With(nameof(_aggregate), _aggregate)
                .Build();

            var path = $@"{_domainDirectory.Value}\Events\{_aggregate.Value}.cs";

            if (!_fileSystem.Exists(path))
            {
                _fileSystem.WriteAllLines(path, GetTemplate(tokens));
            }
            else
            {
                var lines = new List<string>();

                foreach (var line in File.ReadLines(path))
                {
                    if (line.StartsWith("}"))
                    {
                        lines.Add(GetTemplate(tokens, true)[0]);
                    }

                    lines.Add(line);
                }

                _fileSystem.WriteAllLines(path, lines.ToArray());
            }
        }

        private string[] GetTemplate(Dictionary<string, object> tokens, bool append = false)
        {
            if (!append)
            {
                var template = GetTemplate(tokens, true)[0];

                return new string[7] {
                        "using BuildingBlocks.EventStore;",
                        "using System;",
                        "",
                        $"namespace {_domainNamespace.Value}.Events",
                        "{",
                        $"{template}"
                        ,"}"
                    };
            }

            if (_eventName.Value == $"{_aggregate.Value}Removed")
            {
                return new string[1] { $"    public record {_aggregate.PascalCase}Removed (DateTime Deleted):Event;" };
            }

            if (_eventName.Value == $"{_aggregate.Value}Created")
            {
                return new string[1] { $"    public record {_aggregate.PascalCase}Created (Guid {_aggregate.PascalCase}Id):Event;" };
            }

            return new string[1] { $"    public record {_eventName.PascalCase} (string Value):Event;" };
        }
    }
}
