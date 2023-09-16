// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Core.Services;

public interface IClipboardService
{
    void SetText(string value);

    Task<string> GetTextAsync(CancellationToken cancellationToken = default);
}
