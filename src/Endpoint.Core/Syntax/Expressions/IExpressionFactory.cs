// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Expressions;

public interface IExpressionFactory
{
    Task<ExpressionModel> CreateAsync();

    Task<ExpressionModel> LogInformationAsync(string value);

    Task<ExpressionModel> ToDtoCreateAsync(ClassModel aggregate);
}
