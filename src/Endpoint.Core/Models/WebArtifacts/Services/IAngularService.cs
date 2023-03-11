// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public interface IAngularService
{
    void CreateWorkspace(string name, string version, string projectName, string projectType, string prefix, string rootDirectory, bool openInVsCode = true);
    void AddProject(AngularProjectModel model);
    void EnableDefaultStandalone(AngularProjectReferenceModel model);
    void KarmaRemove(string directory);
    void UpdateCompilerOptionsToUseJestTypes(AngularProjectModel model);
    void NgxTranslateAdd(string projectName, string directory);
    void LocalizeAdd(AngularProjectReferenceModel model, List<string> locales);
    void I18nExtract(AngularProjectReferenceModel model);
    void BootstrapAdd(AngularProjectReferenceModel model);
    void PrettierAdd(string directory);
    void ComponentCreate(string name, string directory);
    void ServiceCreate(string name, string directory);
    void ModelCreate(string name, string directory, string properties = null);
    void ListComponentCreate(string name, string directory);
    void DetailComponentCreate(string name, string directory);
    void IndexCreate(bool scss, string directory);
    void DefaultScssCreate(string directory);
    void ScssComponentCreate(string name, string directory);
    void MaterialAdd(AngularProjectReferenceModel model);
    void AddBuildConfiguration(string configurationName, AngularProjectReferenceModel model);
}