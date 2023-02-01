// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Models.Artifacts.Solutions;

public interface IPlantUmlParserStrategyFactory
{
    dynamic CreateFor(string plantUml, dynamic context = null);
}

