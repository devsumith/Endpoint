using Endpoint.Core.Factories;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Services;
using System;
using System.IO;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Core.Strategies.Solutions.Update;

public class SettingsUpdateStrategy : ISettingsUpdateStrategy
{
    private readonly IFileSystem _fileSystem;
    private readonly IEntityModelFactory _entityModelFactory;

    public SettingsUpdateStrategy(IFileSystem fileSystem, IEntityModelFactory entityModelFactory)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _entityModelFactory = entityModelFactory;
    }

    public bool CanHandle(SolutionSettingsModel model, string entityName, string properties)
    {
        return true;
    }

    public void Update(SolutionSettingsModel model, string entityName, string properties)
    {
        model.Entities.Add(_entityModelFactory.Create(entityName, properties));

        var json = Serialize(model, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        _fileSystem.WriteAllText($"{model.Directory}{Path.DirectorySeparatorChar}cliSettings.json", json);
    }

    public int Order => 0;
}
