// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Services;

public interface ICodeFormatterService
{
    Task FormatAsync(string directory);
}
