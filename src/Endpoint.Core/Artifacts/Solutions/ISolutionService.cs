// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts.Solutions;

//https://stackoverflow.com/questions/47637228/how-can-i-add-files-to-a-solution-folder
public interface ISolutionService
{
    void AddSolutionItem(string path);
    void Create(SolutionModel model);
    void Create(string name, string plantUmlSourcePath, string directory);
    SolutionModel CreateFromPlantUml(string plantUml, string name, string directory);
    void EventDrivenMicroservicesCreate(string name, string services, string directory);
    void MessagingBuildingBlockAdd(string directory);
    void IOCompressionBuildingBlockAdd(string directory);
}

