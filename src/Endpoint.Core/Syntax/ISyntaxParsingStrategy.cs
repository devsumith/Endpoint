// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax;

public interface ISyntaxParsingStrategy
{
    Task<object> ParseObjectAsync(ISyntaxParser parser, string value);

    int GetPriority();
}

public interface ISyntaxParsingStrategy<T>: ISyntaxParsingStrategy
    where T : SyntaxModel
{    
    Task<T> ParseAsync(ISyntaxParser parser, string value);
}
