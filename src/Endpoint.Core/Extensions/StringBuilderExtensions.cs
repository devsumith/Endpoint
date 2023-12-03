// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Expressions;
using System.Text;

namespace Endpoint.Core.Extensions;

public static class StringBuilderExtensions { 

    public static StringBuilder AppendDoubleLine(this StringBuilder builder, string value)
        => builder.AppendLine(value)
            .AppendLine();

    public static ExpressionModel ToExpression(this StringBuilder builder)
    {
        return new ExpressionModel(builder.ToString());
    }

}
