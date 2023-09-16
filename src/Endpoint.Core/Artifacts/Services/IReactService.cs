// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Endpoint.Core.Artifacts.React;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Services;

public interface IReactService
{
    void Create(ReactAppReferenceModel model);
}
