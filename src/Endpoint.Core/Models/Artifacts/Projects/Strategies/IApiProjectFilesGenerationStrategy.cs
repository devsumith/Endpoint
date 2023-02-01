// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Models.Artifacts.Projects.Strategies
{
    public interface IApiProjectFilesGenerationStrategy
    {
        void Build(SettingsModel settings);
        void BuildAdditionalResource(string additionalResource, SettingsModel settings);
        void AddGenerateDocumentationFile(string csProjPath);
    }
}

