// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Options;
using Endpoint.Core.Services;
using System;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Core.Strategies
{
    public class SolutionSettingsFileGenerationStrategy : ISolutionSettingsFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;

        public SolutionSettingsFileGenerationStrategy(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public bool? CanHandle(SolutionSettingsModel request) => !request.Metadata.Contains(Constants.SolutionTemplates.Minimal);
        public SolutionSettingsModel Create(SolutionSettingsModel model)
        {
            var json = Serialize(model, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            _fileSystem.WriteAllText(model.Path, json);

            return model;
        }
    }
}

