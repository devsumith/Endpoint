// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Services;

public interface IContext : IDictionary<string, string[]>
{

}

public class Context : Dictionary<string, string[]>, IContext
{
}

