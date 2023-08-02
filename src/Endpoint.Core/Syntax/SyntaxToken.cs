// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax;

public record SyntaxToken
{
    public string Value { get; private init; }
    public SyntaxToken(string value)
    {
        Value = value;
    }

    public static implicit operator string(SyntaxToken token) => token.Value;

    public static explicit operator SyntaxToken(string token) => new SyntaxToken(token);
}

