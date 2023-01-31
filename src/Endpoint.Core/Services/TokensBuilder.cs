using Endpoint.Core.Models.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Services
{
    public class TokensBuilder
    {
        private Dictionary<string, object> _value { get; set; } = new();

        public TokensBuilder()
        {
            _value = new();
        }

        public TokensBuilder With(string propertyName, string value)
            => With(propertyName, (SyntaxToken)value);

        public TokensBuilder With(string propertyName, SyntaxToken token)
        {
            var tokens = token == null ? new SyntaxToken("").ToTokens(propertyName) : token.ToTokens(propertyName);
            _value = new Dictionary<string, object>(_value.Concat(tokens));
            return this;
        }

        public Dictionary<string, object> Build()
            => this._value;
    }
}
