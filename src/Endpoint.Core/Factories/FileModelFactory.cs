using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Factories
{
    public class FileModelFactory
    {
        public static FileModel CreateCSharp(string template, string @namespace, string name, string directory, Dictionary<string, object> tokens = null)
            => new()
            {
                Template = template,
                Namespace = @namespace,
                Name = name,
                Directory = directory,
                Extension = "cs",
                Tokens = tokens ?? new TokensBuilder()
                .With("Name", (Token)name)
                .With("Namespace", (Token)@namespace)
                .Build()
            };
    }
}
