// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Solutions;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public interface ISolutionUpdateStrategyFactory
    {
        void UpdateFor(SolutionModel previous, SolutionModel next);
    }
}

