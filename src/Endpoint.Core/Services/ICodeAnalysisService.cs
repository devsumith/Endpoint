// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Solutions;

namespace Endpoint.Core.Services;

public interface ICodeAnalysisService
{
    SyntaxModel SyntaxModel { get; set; }

    SolutionModel SolutionModel { get; set; }
}
