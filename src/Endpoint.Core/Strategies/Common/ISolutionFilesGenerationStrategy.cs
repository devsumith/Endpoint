// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Options;
using System.Collections.Generic;

namespace Endpoint.Core.Services
{
    public interface ISolutionFilesGenerationStrategy
    {
        public SettingsModel Build(string name, string properties, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, string resource, string directory, bool isMicroserviceArchitecture, List<string> plugins, string prefix);

        public SettingsModel Build(string name, string properties, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, List<string> resources, string directory, bool isMicroserviceArchitecture, List<string> plugins, string prefix);

        public SettingsModel Create(SettingsModel model);
    }
}

