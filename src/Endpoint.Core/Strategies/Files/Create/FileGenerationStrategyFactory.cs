// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Files;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies.Files.Create
{

    public class FileGenerator : IFileGenerator
    {
        private readonly IEnumerable<IFileGenerationStrategy> _strategies;

        public FileGenerator(
            IEnumerable<IFileGenerationStrategy> strategies
            )
        {
            _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        }

        public void CreateFor(FileModel model)
        {
            var strategy = _strategies.Where(x => x.CanHandle(model)).OrderByDescending(x => x.Order).FirstOrDefault();

            strategy.Create(model);
        }
    }
}

