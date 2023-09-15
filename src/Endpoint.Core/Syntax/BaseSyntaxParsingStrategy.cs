// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax;

public abstract class BaseSyntaxParsingStrategy<T> : ISyntaxParsingStrategy<T>
    where T : SyntaxModel
{
    public virtual int GetPriority() => 0;

    public abstract Task<T> ParseAsync(ISyntaxParser parser, string value);

    public virtual async Task<object> ParseObjectAsync(ISyntaxParser parser, string value)
        => await ParseAsync(parser, value);
}
