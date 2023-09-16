// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Options;


namespace Endpoint.Core.Artifacts.Projects.Strategies;

public interface IApiProjectFilesGenerationStrategy
{
    void Build(dynamic settings);
    void BuildAdditionalResource(string additionalResource, dynamic settings);
    void AddGenerateDocumentationFile(string csProjPath);
}


