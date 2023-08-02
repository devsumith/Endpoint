
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.Types;
using Endpoint.Core.Strategies.Common;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Entities.Legacy;
using Endpoint.Core.Syntax.Properties;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;
using Endpoint.Core.Options;


namespace Endpoint.Core.Services;

public class SolutionFilesGenerationStrategy : ISolutionFilesGenerationStrategy
{
    protected readonly ICommandService _commandService;
    protected readonly ITemplateProcessor _templateProcessor;
    protected readonly ITemplateLocator _templateLocator;
    protected readonly IFileSystem _fileSystem;

    public SolutionFilesGenerationStrategy(
        ICommandService commandService,
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        IFileSystem fileSystem)
    {
        _commandService = commandService;
        _templateProcessor = templateProcessor;
        _templateLocator = templateLocator;
        _fileSystem = fileSystem;
    }

    public SettingsModel Build(string name, string properties, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, string resource, string directory, bool isMicroserviceArchitecture, List<string> plugins, string prefix)
    {
        LegacyAggregatesModel aggregateRoot = new LegacyAggregatesModel(resource);

        aggregateRoot.Properties.Add(new PropertyModel(aggregateRoot, AccessModifier.Public, new TypeModel() { Name = "Guid" }, $"{((SyntaxToken)resource).PascalCase}Id", PropertyAccessorModel.GetPrivateSet, key: true));

        if (!string.IsNullOrWhiteSpace(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var nameValuePair = property.Split(':');

                aggregateRoot.Properties.Add(new PropertyModel(aggregateRoot, AccessModifier.Public, new TypeModel() { Name = nameValuePair.ElementAt(1) }, nameValuePair.ElementAt(0), PropertyAccessorModel.GetPrivateSet));
            }
        }

        return Build(name, dbContextName, useShortIdProperty, useIntIdPropertyType, new List<LegacyAggregatesModel>() { aggregateRoot }, directory, isMicroserviceArchitecture, plugins, prefix);
    }

    public SettingsModel Build(string name, string properties, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, List<string> resources, string directory, bool isMicroserviceArchitecture, List<string> plugins, string prefix)
    {
        var aggregates = new List<LegacyAggregatesModel>();

        foreach (var resource in resources)
        {
            LegacyAggregatesModel aggregateRoot = new LegacyAggregatesModel(resource);

            var idPropertyName = useShortIdProperty ? "Id" : $"{((SyntaxToken)resource).PascalCase}Id";

            var idDotNetType = useIntIdPropertyType ? "int" : "Guid";

            aggregateRoot.Properties.Add(new PropertyModel(aggregateRoot, AccessModifier.Public, new TypeModel() { Name = idDotNetType }, idPropertyName, PropertyAccessorModel.GetPrivateSet, key: true));

            if (!string.IsNullOrWhiteSpace(properties))
            {
                foreach (var property in properties.Split(','))
                {
                    var nameValuePair = property.Split(':');

                    aggregateRoot.Properties.Add(new PropertyModel(aggregateRoot, AccessModifier.Public, new TypeModel() { Name = nameValuePair.ElementAt(1) }, nameValuePair.ElementAt(0), PropertyAccessorModel.GetPrivateSet));
                }
            }

            aggregates.Add(aggregateRoot);
        }


        return Build(name, dbContextName, useShortIdProperty, useIntIdPropertyType, aggregates, directory, isMicroserviceArchitecture, plugins, prefix);

    }

    public SettingsModel Build(string name, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, List<LegacyAggregatesModel> resources, string directory, bool isMicroserviceArchitecture, List<string> plugins, string prefix)
    {

        name = name.Replace("-", "_");

        _fileSystem.CreateDirectory($"{directory}{Path.DirectorySeparatorChar}{name}");

        var settings = new SettingsModel(name, dbContextName, resources, directory, isMicroserviceArchitecture, plugins, useShortIdProperty ? IdPropertyFormat.Short : IdPropertyFormat.Long, useIntIdPropertyType ? IdPropertyType.Int : IdPropertyType.Guid, prefix);

        return Create(settings);
    }

    public SettingsModel Create(SettingsModel settings)
    {
        var json = Serialize(settings, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        _fileSystem.CreateDirectory($"{settings.RootDirectory}");

        _fileSystem.WriteAllText($"{settings.RootDirectory}{Path.DirectorySeparatorChar}{Constants.SettingsFileName}", json);

        _commandService.Start($"dotnet new sln -n {settings.SolutionName}", settings.RootDirectory);

        _fileSystem.CreateDirectory($"{settings.RootDirectory}{Path.DirectorySeparatorChar}{settings.SourceFolder}");

        _fileSystem.CreateDirectory($"{settings.RootDirectory}{Path.DirectorySeparatorChar}{settings.TestFolder}");

        _fileSystem.CreateDirectory($"{settings.RootDirectory}{Path.DirectorySeparatorChar}deploy");

        _commandService.Start("git init", settings.RootDirectory);

        new GitIgnoreFileGenerationStrategy(_fileSystem, _templateLocator).Generate(settings);

        if (settings.IsMicroserviceArchitecture)
        {
            _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.WebApi, settings.ApiDirectory, settings);

            _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.ClassLibrary, settings.TestingDirectory, settings);

            _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.XUnit, settings.UnitTestsDirectory, settings);

            _addReference(settings.TestingDirectory, settings.ApiDirectory);

            _addReference(settings.UnitTestsDirectory, settings.TestingDirectory);
        }
        else
        {
            _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.WebApi, settings.ApiDirectory, settings);

            _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.ClassLibrary, settings.DomainDirectory, settings);

            _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.ClassLibrary, settings.ApplicationDirectory, settings);

            _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.ClassLibrary, settings.InfrastructureDirectory, settings);

            _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.ClassLibrary, settings.TestingDirectory, settings);

            _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.XUnit, settings.UnitTestsDirectory, settings);

            _addReference(settings.ApplicationDirectory, settings.DomainDirectory);

            _addReference(settings.InfrastructureDirectory, settings.ApplicationDirectory);

            _addReference(settings.ApiDirectory, settings.InfrastructureDirectory);

            _addReference(settings.TestingDirectory, settings.ApiDirectory);

            _addReference(settings.TestingDirectory, settings.ApiDirectory);

            _addReference(settings.UnitTestsDirectory, settings.TestingDirectory);
        }

        return settings;
    }

    private void _createProjectAndAddToSolution(string templateType, string directory, SettingsModel settings)
    {

        _fileSystem.CreateDirectory(directory);

        _commandService.Start($"dotnet new {templateType} --framework net6.0", directory);

        var parts = directory.Split(Path.DirectorySeparatorChar);

        var name = parts[parts.Length - 1];

        _commandService.Start($"dotnet sln add {directory}{Path.DirectorySeparatorChar}{name}.csproj", settings.RootDirectory);
    }

    private void _addReference(string targetDirectory, string referencedDirectory)
    {
        var parts = referencedDirectory.Split(Path.DirectorySeparatorChar);

        var name = parts[parts.Length - 1];

        _commandService.Start($"dotnet add {targetDirectory} reference {referencedDirectory}{Path.DirectorySeparatorChar}{name}.csproj");
    }
}

