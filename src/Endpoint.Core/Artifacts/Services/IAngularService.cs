// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Endpoint.Core.Artifacts.AngularProjects;

namespace Endpoint.Core.Artifacts.Services;

public interface IAngularService
{
    Task CreateWorkspace(string name, string version, string projectName, string projectType, string prefix, string rootDirectory, bool openInVsCode = true);
    Task AddProject(AngularProjectModel model);
    Task EnableDefaultStandalone(AngularProjectReferenceModel model);
    Task KarmaRemove(string directory);
    Task UpdateCompilerOptionsToUseJestTypes(AngularProjectModel model);
    Task NgxTranslateAdd(string projectName, string directory);
    Task LocalizeAdd(AngularProjectReferenceModel model, List<string> locales);
    Task I18nExtract(AngularProjectReferenceModel model);
    Task BootstrapAdd(AngularProjectReferenceModel model);
    Task PrettierAdd(string directory);
    Task ComponentCreate(string name, string directory);
    Task ServiceCreate(string name, string directory);
    Task ModelCreate(string name, string directory, string properties = null);
    Task ListComponentCreate(string name, string directory);
    Task DetailComponentCreate(string name, string directory);
    Task IndexCreate(bool scss, string directory);
    Task DefaultScssCreate(string directory);
    Task ScssComponentCreate(string name, string directory);
    Task MaterialAdd(AngularProjectReferenceModel model);
    Task AddBuildConfiguration(string configurationName, AngularProjectReferenceModel model);
    Task ControlCreate(string name, string directory);
    Task Test(string directory, string searchPattern = "*.spec.ts");
}