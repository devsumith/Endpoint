// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts.Files;

public class LaunchSettingsFileModel : FileModel
{
    public LaunchSettingsFileModel(string directory)
        : base("launchSettings", directory, ".json")
    {
    }
}
