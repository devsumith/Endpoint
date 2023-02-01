// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Endpoint.Core.Services;

public interface ITokenBuilder
{
    Dictionary<string, object> Build(IDictionary<string, string> args, string directory);
    string Get(string key, IDictionary<string, string> args);
}

