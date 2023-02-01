// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Models.WebArtifacts.Services;

public interface ISignalRService
{
    void Add(string directory);

    void AddHub(string name, string directory);

}


