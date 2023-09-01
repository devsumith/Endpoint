// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Services;

public interface ITemplateProcessor
{
    string Process(string template, IDictionary<string, object> tokens, string[] ignoreTokens = null);
    string Process(string template, IDictionary<string, object> tokens);
    string Process(string template, dynamic model);

    Task<string> ProcessAsync(string template, IDictionary<string, object> tokens, string[] ignoreTokens = null);
    Task<string> ProcessAsync(string template, IDictionary<string, object> tokens);
    Task<string> ProcessAsync(string template, dynamic model);
}
