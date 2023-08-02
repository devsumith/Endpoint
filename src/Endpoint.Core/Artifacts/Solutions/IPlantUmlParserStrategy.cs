// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts.Solutions;

public interface IPlantUmlParserStrategy
{
    bool CanHandle(string plantUml);
    int Priority { get; }
    object Create(string plantUml, dynamic context = null);
}

