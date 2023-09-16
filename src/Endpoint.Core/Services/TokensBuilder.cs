// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Endpoint.Core.Syntax;

namespace Endpoint.Core.Services;

public class TokensBuilder
{
    public TokensBuilder()
    {
        Value = new ();
    }

    private Dictionary<string, object> Value { get; set; } = new ();

    public TokensBuilder With(string propertyName, string value)
        => With(propertyName, (SyntaxToken)value);

    public TokensBuilder With(string propertyName, SyntaxToken token)
    {
        var tokens = token == null ? new SyntaxToken(string.Empty).ToTokens(propertyName) : token.ToTokens(propertyName);
        Value = new Dictionary<string, object>(Value.Concat(tokens));
        return this;
    }

    public Dictionary<string, object> Build()
        => this.Value;
}
