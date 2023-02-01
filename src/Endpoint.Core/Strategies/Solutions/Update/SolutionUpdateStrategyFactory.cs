// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public class SolutionUpdateStrategyFactory : ISolutionUpdateStrategyFactory
    {
        private readonly IEnumerable<ISolutionUpdateStrategy> _solutionUpdateStrategies;

        public SolutionUpdateStrategyFactory(IEnumerable<ISolutionUpdateStrategy> solutionUpdateStrategies)
        {
            _solutionUpdateStrategies = solutionUpdateStrategies;
        }

        public void UpdateFor(SolutionModel previous, SolutionModel next)
        {
            if (previous == null)
            {
                throw new ArgumentNullException(nameof(previous));
            }

            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }



            var strategy = _solutionUpdateStrategies.Where(x => x.CanHandle(previous, next)).OrderByDescending(x => x.Order).FirstOrDefault();

            if (strategy == null)
            {
                throw new InvalidOperationException("Cannot find a strategy for generation for the type ");
            }

            strategy.Update(previous, next);
        }
    }
}

