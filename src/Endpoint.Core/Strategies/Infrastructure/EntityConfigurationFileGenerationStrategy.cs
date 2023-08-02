// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using System;
using System.IO;

namespace Endpoint.Core.Strategies.Infrastructure
{
    public interface IEntityConfigurationFileGenerationStrategy
    {
        void Create(string entityName, string idPropertyName, string applicationNamespace, string infrastructureNamespace, string directory);
    }

    public class EntityConfigurationFileGenerationStrategy : IEntityConfigurationFileGenerationStrategy
    {
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly IFileSystem _fileSystem;

        public EntityConfigurationFileGenerationStrategy(
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem
            )
        {
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public void Create(string entityName, string idPropertyName, string applicationNamespace, string infrastructureNamespace, string directory)
        {
            var tokens = new TokensBuilder()
                .With(nameof(applicationNamespace), (SyntaxToken)applicationNamespace)
                .With(nameof(infrastructureNamespace), (SyntaxToken)infrastructureNamespace)
                .With(nameof(idPropertyName), (SyntaxToken)idPropertyName)
                .With("EntityName", (SyntaxToken)entityName)
                .Build();

            var content = _templateProcessor.Process(_templateLocator.Get("EntityConfiguration"), tokens);

            _fileSystem.WriteAllText($"{directory}{Path.DirectorySeparatorChar}{((SyntaxToken)entityName).PascalCase}EntityConfiguration.cs", string.Join(Environment.NewLine, content));
        }
    }
}

