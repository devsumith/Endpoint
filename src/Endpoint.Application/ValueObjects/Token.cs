using CSharpFunctionalExtensions;
using Endpoint.Application.Services;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Endpoint.Application.ValueObjects
{
    public class Token : ValueObject
    {
        private readonly NamingConventionConverter namingConventionConverter = new NamingConventionConverter();

        public const int MaxLength = 250;
        [JsonProperty]
        public string Value { get; private set; }

        public Token(string value = "")
        {
            Value = value;
        }

        public string PascalCase => namingConventionConverter.Convert(NamingConvention.PascalCase, Value);
        public string PascalCasePlural => namingConventionConverter.Convert(NamingConvention.PascalCase, Value, pluralize: true);
        public string CamelCase => namingConventionConverter.Convert(NamingConvention.CamelCase, Value);
        public string CamelCasePlural => namingConventionConverter.Convert(NamingConvention.CamelCase, Value, pluralize: true);
        public string SnakeCase => namingConventionConverter.Convert(NamingConvention.SnakeCase, Value);
        public string SnakeCasePlural => namingConventionConverter.Convert(NamingConvention.SnakeCase, Value, pluralize: true);
        public string TitleCase => namingConventionConverter.Convert(NamingConvention.TitleCase, Value);

        public Dictionary<string, object> ToTokens(string propertyName)
        {
            propertyName = propertyName.Substring(propertyName.IndexOf('_') + 1);

            var propertyNameCamelCase = ((Token)propertyName).CamelCase;
            return new()
            {
                { $"{propertyName}", Value },
                { $"{propertyNameCamelCase}PascalCase", PascalCase },
                { $"{propertyNameCamelCase}PascalCasePlural", PascalCasePlural },
                { $"{propertyNameCamelCase}CamelCase", CamelCase },
                { $"{propertyNameCamelCase}CamelCasePlural", CamelCasePlural },
                { $"{propertyNameCamelCase}SnakeCase", SnakeCase },
                { $"{propertyNameCamelCase}SnakeCasePlural", SnakeCasePlural },
                { $"{propertyNameCamelCase}TitleCase", TitleCase }
            };
        }

        public static Result<Token> Create(string value)
        {
            value = (value ?? string.Empty).Trim();

            return Result.Success(new Token(value));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(Token token)
        {
            return token.Value;
        }

        public static explicit operator Token(string token)
        {
            return Create(token).Value;
        }
    }
}
