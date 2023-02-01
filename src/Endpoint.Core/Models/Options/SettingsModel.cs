// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Entities.Legacy;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Core.Models.Options;

public class SettingsModel
{
    public bool IsRoot { get; set; } = true;
    public bool Minimal { get; set; } = false;
    public string Prefix { get; set; } = "app";
    public IdPropertyFormat IdFormat { get; set; } = IdPropertyFormat.Long;
    public IdPropertyType IdDotNetType { get; set; } = IdPropertyType.Guid;
    public bool IsMicroserviceArchitecture { get; set; } = true;
    public List<string> Projects { get; set; } = new List<string>();
    public string RootDirectory { get; set; }
    public string SolutionName { get; set; }
    public string SolutionFileName { get; set; }
    public string ApiFullPath { get; set; }
    public string RootNamespace { get; set; }
    public string DomainNamespace { get; set; }
    public string ApplicationNamespace { get; set; }
    public string InfrastructureNamespace { get; set; }
    public string ApiNamespace { get; set; }
    public string DomainDirectory { get; set; }
    public string ApplicationDirectory { get; set; }
    public string InfrastructureDirectory { get; set; }
    public string ApiDirectory { get; set; }
    public string UnitTestsDirectory { get; set; }
    public string TestingDirectory { get; set; }
    public List<string> AppDirectories { get; set; } = new List<string>();
    public string BuildingBlocksCoreNamespace { get; set; } = "BuildingBlocks.Core";
    public string BuildingBlocksEventStoreNamespace { get; set; } = "BuildingBlocks.EventStore";
    public string SourceFolder { get; set; } = "src";
    public string TestFolder { get; set; } = "tests";
    public string DbContextName { get; set; }
    public int? Port { get; set; } = 5000;
    public int? SslPort { get; set; } = 5001;
    public List<string> Plugins { get; set; }
    public List<EntityModel> Entities { get; set; } = new List<EntityModel>();
    public List<LegacyAggregateModel> Resources { get; set; } = new List<LegacyAggregateModel>();
    public string Directory { get; set; }
    public List<string> Metadata { get; set; } = new List<string>();
    public SettingsModel(string name, string dbContextName, LegacyAggregateModel resource, string directory, bool isMicroserviceArchitecture = true, List<string> plugins = default, IdPropertyFormat idFormat = IdPropertyFormat.Long, IdPropertyType idDotNetType = IdPropertyType.Guid, string prefix = "app", bool minimal = false)
        : this(name, dbContextName, new List<LegacyAggregateModel>() { resource }, directory, isMicroserviceArchitecture, plugins, idFormat, idDotNetType, prefix, minimal)
    { }

    public SettingsModel(string name, string dbContextName, List<LegacyAggregateModel> resources, string directory, bool isMicroserviceArchitecture = true, List<string> plugins = default, IdPropertyFormat idFormat = IdPropertyFormat.Long, IdPropertyType idDotNetType = IdPropertyType.Guid, string prefix = "app", bool minimal = false)
    {
        name = ((SyntaxToken)name).PascalCase().Replace("-", "_");
        Plugins = plugins;
        IdDotNetType = idDotNetType;
        IdFormat = idFormat;
        Prefix = prefix;
        Minimal = minimal;

        foreach (var resource in resources)
        {
            Resources.Add(resource);
        }

        IsMicroserviceArchitecture = isMicroserviceArchitecture;

        SolutionName = name;
        SolutionFileName = $"{name}.sln";


        var parts = name.Split('.');
        DbContextName = dbContextName ?? $"{parts[parts.Length - 1]}DbContext";


        RootDirectory = $"{directory}{Path.DirectorySeparatorChar}{SolutionName}";
        RootNamespace = SolutionName;
        ApiNamespace = $"{RootNamespace}.Api";
        ApiFullPath = $"{RootDirectory}{Path.DirectorySeparatorChar}{SourceFolder}{Path.DirectorySeparatorChar}{ApiNamespace}{Path.DirectorySeparatorChar}{ApiNamespace}.csproj";
        InfrastructureNamespace = IsMicroserviceArchitecture ? $"{RootNamespace}.Api" : $"{RootNamespace}.Infrastructure";
        DomainNamespace = IsMicroserviceArchitecture ? $"{RootNamespace}.Api" : $"{RootNamespace}.Core";
        ApplicationNamespace = IsMicroserviceArchitecture ? $"{RootNamespace}.Api" : $"{RootNamespace}.Core";

        var sourceFolder = $"{RootDirectory}{Path.DirectorySeparatorChar}{SourceFolder}{Path.DirectorySeparatorChar}";
        var testsFolder = $"{RootDirectory}{Path.DirectorySeparatorChar}{TestFolder}{Path.DirectorySeparatorChar}";

        if (IsMicroserviceArchitecture)
        {
            ApiDirectory = $"{sourceFolder}{SolutionName}.Api";
            InfrastructureDirectory = $"{sourceFolder}{SolutionName}.Api";
            DomainDirectory = $"{sourceFolder}{SolutionName}.Api";
            ApplicationDirectory = $"{sourceFolder}{SolutionName}.Api";
        }
        else
        {
            ApiDirectory = $"{RootDirectory}{Path.DirectorySeparatorChar}{SourceFolder}{Path.DirectorySeparatorChar}{SolutionName}.Api";
            InfrastructureDirectory = $"{RootDirectory}{Path.DirectorySeparatorChar}{SourceFolder}{Path.DirectorySeparatorChar}{SolutionName}.Infrastructure";
            DomainDirectory = $"{RootDirectory}{Path.DirectorySeparatorChar}{SourceFolder}{Path.DirectorySeparatorChar}{SolutionName}.SharedKernel";
            ApplicationDirectory = $"{RootDirectory}{Path.DirectorySeparatorChar}{SourceFolder}{Path.DirectorySeparatorChar}{SolutionName}.Core";
        }

        UnitTestsDirectory = $"{testsFolder}{RootNamespace}.UnitTests";
        TestingDirectory = $"{testsFolder}{RootNamespace}.Testing";

    }

    public void AddResource(string resource, string properties, IFileSystem fileSystem)
    {
        var aggregate = new LegacyAggregateModel(resource);

        aggregate.IdPropertyName = IdFormat == IdPropertyFormat.Short ? "Id" : $"{resource}Id";

        aggregate.IdPropertyType = IdDotNetType == IdPropertyType.Int ? "int" : "Guid";

        aggregate.Properties.Add(new PropertyModel(aggregate, AccessModifier.Public, new TypeModel() { Name = aggregate.IdPropertyType }, aggregate.IdPropertyName, PropertyAccessorModel.GetPrivateSet, key: true));

        if (!string.IsNullOrEmpty(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var nameValuePair = property.Split(':');

                if (nameValuePair.ElementAt(0) != aggregate.IdPropertyName)
                {
                    aggregate.Properties.Add(new PropertyModel(aggregate, AccessModifier.Public, new TypeModel() { Name = nameValuePair.ElementAt(1) }, nameValuePair.ElementAt(0), PropertyAccessorModel.GetPrivateSet));
                }
            }
        }

        AddResource(aggregate, fileSystem);
    }

    public void AddResource(LegacyAggregateModel aggregate, IFileSystem fileSystem)
    {
        if (Resources.FirstOrDefault(x => x.Name == aggregate.Name) == null)
        {
            Resources.Add(aggregate);
        }

        PersistToFileSystem(fileSystem);
    }

    public void PersistToFileSystem(IFileSystem fileSystem)
    {
        fileSystem.WriteAllLines($"{RootDirectory}{Path.DirectorySeparatorChar}clisettings.json", new string[1] {
                Serialize(this, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    })
            });
    }

    public void AddApp(string directory, IFileSystem fileSystem)
    {
        if (!AppDirectories.Contains(directory))
        {
            AppDirectories = AppDirectories.Concat(new string[1] { directory }).ToList();
        }

        fileSystem.WriteAllLines($"{RootDirectory}{Path.DirectorySeparatorChar}clisettings.json", new string[1] {
                Serialize(this, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    })
            });
    }

    public SettingsModel()
    {

    }

}

