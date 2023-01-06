﻿using Endpoint.Core.Models.Artifacts;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Services;

namespace Endpoint.Core.Strategies.Files.Create;

public class EntityFileGenerationStrategy : IFileGenerationStrategy
{
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateLocator _templateLocator;
    private readonly ITemplateProcessor _templateProcessor;

    public EntityFileGenerationStrategy(IFileSystem fileSystem, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
    }

    public int Order => 0;

    public bool CanHandle(FileModel model) => model is EntityFileModel;

    public void Create(dynamic model)
    {
        if(model is EntityFileModel entityFileModel)
        {
            var template = _templateLocator.Get(nameof(EntityModel));


            var result = _templateProcessor.Process(template, new { });

            _fileSystem.WriteAllText(entityFileModel.Path, result);
        }
     }

}