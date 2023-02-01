// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Solutions;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public interface ISolutionUpdateStrategy
    {
        bool CanHandle(SolutionModel previous, SolutionModel next);
        void Update(SolutionModel previous, SolutionModel next);
        int Order { get; set; }
    }
}

