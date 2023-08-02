// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Options;


namespace Endpoint.Core.Strategies.Solutions.Update;

public interface ISettingsUpdateStrategy
{
    bool CanHandle(SolutionSettingsModel model, string entityName, string properties);
    void Update(SolutionSettingsModel model, string entityName, string properties);

    int Order { get; }
}


