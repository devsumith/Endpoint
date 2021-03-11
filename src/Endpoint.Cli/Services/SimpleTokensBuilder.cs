using Endpoint.Cli.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Cli.Services
{
    public class SimpleTokensBuilder 
    {
        private Dictionary<string, object> _value { get; set; } = new();

        public SimpleTokensBuilder()
        {
            _value = new();
        }
        public SimpleTokensBuilder WithToken(string propertyName, Token token)
        {
            var tokens = token.ToTokens(propertyName);
            _value = new Dictionary<string, object>(_value.Concat(tokens));
            return this;
        }

        public Dictionary<string,object> Build()
            => this._value;
    }
}
