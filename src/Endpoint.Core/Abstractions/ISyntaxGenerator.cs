// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Endpoint.Core.Abstractions;

public interface ISyntaxGenerator
{
    string CreateFor(object model, dynamic context = null);
    Task<string> CreateAsync(object model, dynamic context = null);
}

