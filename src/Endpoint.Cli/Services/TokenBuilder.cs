using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Endpoint.Cli.Services
{
    public class TokenBuilder : ITokenBuilder
    {
        private readonly INamingConventionConverter _namingConventionConverter;
        private readonly ISettingsProvider _settingsProvider;
        private readonly INamespaceProvider _namespaceProvider;
        public TokenBuilder(INamingConventionConverter namingConventionConverter, ISettingsProvider settingsProvider, INamespaceProvider namespaceProvider)
        {
            _namingConventionConverter = namingConventionConverter;
            _settingsProvider = settingsProvider;
            _namespaceProvider = namespaceProvider;
        }

        public Dictionary<string, object> Build(IDictionary<string,string> args, string directory)
        {
            var options = new ConfigurationBuilder()
                .AddInMemoryCollection(args).Build();

            var entityNamePascalCase = _namingConventionConverter.Convert(NamingConvention.PascalCase, options["Entity"]);

            var @namespace = _namespaceProvider.GetFileNamespace(directory);
            
            var settings = _settingsProvider.Get();
            
            string entityId = !string.IsNullOrEmpty(options["Entity"]) && !string.IsNullOrEmpty(settings.EntityIdDataType) && settings.EntityIdDataType == "Short"
                ? "id"
                : $"{options["Entity"]}Id";

            var tokens = new Dictionary<string, object>
                {
                    { "projectDirectory", options["ProjectDirectory"] },
                    { "directory", directory },
                    { "name", options["Name"] },
                    { "port", options["Port"] },
                    { "sslPort", options["SslPort"] },
                    { "nameSnakeCase", _namingConventionConverter.Convert(NamingConvention.SnakeCase, options["Name"]) },
                    { "nameSnakeCasePlural", _namingConventionConverter.Convert(NamingConvention.SnakeCase, options["Name"], pluralize: true) },
                    { "nameCamelCase", _namingConventionConverter.Convert(NamingConvention.CamelCase, options["Name"]) },
                    { "namePascalCase", _namingConventionConverter.Convert(NamingConvention.PascalCase,options["Name"]) },
                    { "nameTitleCase", _namingConventionConverter.Convert(NamingConvention.TitleCase,options["Name"]) },
                    { "entityNamePascalCasePlural", _namingConventionConverter.Convert(NamingConvention.PascalCase, options["Entity"], pluralize: true) },
                    { "entityNameCamelCasePlural", _namingConventionConverter.Convert(NamingConvention.CamelCase, options["Entity"], pluralize: true) },
                    { "entityNameSnakeCasePlural", _namingConventionConverter.Convert(NamingConvention.SnakeCase, options["Entity"], pluralize: true) },                    
                    { "entityNamePascalCase", entityNamePascalCase },
                    { "entityNameCamelCase", _namingConventionConverter.Convert(NamingConvention.CamelCase, options["Entity"]) },
                    { "entityNameSnakeCase", _namingConventionConverter.Convert(NamingConvention.SnakeCase, options["Entity"]) },
                    { "entityNameTitleCase", _namingConventionConverter.Convert(NamingConvention.TitleCase, options["Entity"]) },
                    { "namespace", @namespace },
                    { "rootNamespace", settings?.RootNamespace },
                    { "rootNamespaceSnakeCase",  _namingConventionConverter.Convert(NamingConvention.SnakeCase, settings?.RootNamespace) },
                    { "entityNameKebobCase", _namingConventionConverter.Convert(NamingConvention.KebobCase, options["Entity"]) },
                    { "entityIdCamelCase", _namingConventionConverter.Convert(NamingConvention.CamelCase, entityId) },
                    { "entityIdPascalCase", _namingConventionConverter.Convert(NamingConvention.PascalCase, entityId) },
                    { "dbContextPascalCase", _namingConventionConverter.Convert(NamingConvention.PascalCase, $"{settings?.RootNamespace?.Replace(".","")}DbContext") },
                    { "dbContextCamelCase", _namingConventionConverter.Convert(NamingConvention.CamelCase, $"{settings?.RootNamespace?.Replace(".","")}DbContext") },
                    { "entityIdCamelCaseInsideCurly", "{" + $"{_namingConventionConverter.Convert(NamingConvention.CamelCase, options["Entity"])}" + "Id}" }
                };

            return tokens;
        }

        public string Get(string key, IDictionary<string,string> args)
        {
            var tokens = Build(args, null);
            tokens.TryGetValue(key, out object value);
            return value as string;
        }
    }
}
