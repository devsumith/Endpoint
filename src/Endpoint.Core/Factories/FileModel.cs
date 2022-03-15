using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public partial class FileModel
    {
        public static FileModel CreateCSharp(string template, string @namespace, string name, string directory, Dictionary<string, object> tokens = null)
        {
            var model = new FileModel();

            model.Template = template;
            model.Namespace = @namespace;
            model.Name = name;
            model.Directory = directory;
            model.Extension = "cs";
            model.Tokens = tokens ?? new TokensBuilder()
                .With(nameof(model.Name), (Token)model.Name)
                .With(nameof(model.Namespace), (Token)model.Namespace)
                .Build();
            return model;
        }
    }
}
