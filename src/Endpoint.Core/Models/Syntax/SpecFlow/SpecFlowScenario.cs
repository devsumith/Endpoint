// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.SpecFlow;

public class SpecFlowScenarioModel {

    public SpecFlowScenarioModel()
    {
        And = new List<string>();
    }
    public string Given { get; init; }
    public string When { get; init; }
    public string Then { get; init; }
    public List<string> And { get; init; }
}

