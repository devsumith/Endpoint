// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public interface IAngularService
{
    void CreateWorkspace(string name, string projectName, string projectType, string prefix, string rootDirectory);
    void AddProject(AngularProjectModel model);
    void EnableDefaultStandaloneComponents(AngularProjectReferenceModel model);
    void KarmaRemove(string directory); 
    void UpdateCompilerOptionsToUseJestTypes(AngularProjectModel model);
    void NgxTranslateAdd(string projectName, string directory);
    void LocalizeAdd(AngularProjectReferenceModel model, List<string> locales);
    void I18nExtract(AngularProjectReferenceModel model);
    void BootstrapAdd(AngularProjectReferenceModel model);
    void PrettierAdd(string directory);
    public void ComponentCreate(string name, string directory);
    public void ServiceCreate(string name, string directory);
}


