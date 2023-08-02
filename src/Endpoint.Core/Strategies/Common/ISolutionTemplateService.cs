// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using System.Collections.Generic;


namespace Endpoint.Core.Services;

public interface ISolutionTemplateService
{
    void Build(string name, string dbContextName, bool shortIdPropertyName, string resource, string properties, bool isMonolith, bool numericIdPropertyDataType, string directory, List<string> plugins, string prefix);
}


