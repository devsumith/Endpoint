// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Properties;
using System.Collections.Generic;

namespace Endpoint.Core.Services;

public interface IDomainDrivenDesignFileService
{
    void ServiceCreate(string name, string directory);

    void MessageCreate(string name, List<PropertyModel> properties, string directory);

    void MessageHandlerCreate(string name, string directory);
}

