// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Files;

namespace Endpoint.Core.Strategies.Files.Create
{
    public interface IFileGenerator
    {
        void CreateFor(FileModel model);
    }
}

