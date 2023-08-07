// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Constructors;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.Types;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Endpoint.Core.Artifacts.Projects.Factories;

public class ProjectFactory : IProjectFactory
{
    private readonly IFileFactory _fileFactory;
    private readonly ILogger<ProjectFactory> _logger;

    public ProjectFactory(IFileFactory fileFactory, ILogger<ProjectFactory> logger)
    {
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ProjectModel> CreateSpecFlowProject(string name, string directory)
    {
        var model = await CreateLibrary(name, directory);

        model.DotNetProjectType = DotNetProjectType.XUnit;

        model.Packages.Add(new() { Name = "SpecFlow.XUnit" });
        model.Packages.Add(new() { Name = "Microsoft.Extensions.Configuration.Json" });
        model.Packages.Add(new() { Name = "FluentAssertions" });
        model.Packages.Add(new() { Name = "Ductus.FluentDocker" });

        return model;
    }

    public async Task<ProjectModel> CreatePlaywrightProject(string name, string directory)
    {
        var model = await CreateLibrary(name, directory);

        model.DotNetProjectType = DotNetProjectType.NUnit;

        model.Packages.Add(new() { Name = "Microsoft.Playwright.NUnit" });
        model.Packages.Add(new() { Name = "SpecFlow.NUnit" });

        return model;
    }

    public async Task<ProjectModel> CreateHttpProject(string name, string directory)
    {
        var model = new ProjectModel(DotNetProjectType.Console, name, directory);

        model.Files.Add(_fileFactory.CreateCSharp("EmptyProgram", "", "Program", model.Directory));
        model.Files.Add(_fileFactory.CreateCSharp("HttpClientExtensions", "", "HttpClientExtensions", model.Directory));
        model.Files.Add(_fileFactory.CreateCSharp("HttpClientFactory", "", "HttpClientFactory", model.Directory));

        return model;
    }

    public async Task<ProjectModel> CreateMinimalApiProject(CreateMinimalApiProjectOptions options)
    {
        var model = new ProjectModel(DotNetProjectType.MinimalWebApi, options.Name, options.Directory);

        var entities = new List<EntityModel> { new EntityModel(options.Resource) };

        model.GenerateDocumentationFile = true;

        model.Files.Add(_fileFactory.LaunchSettingsJson(model.Directory, model.Name, options.Port.Value));
        model.Files.Add(_fileFactory.AppSettings(model.Directory, model.Name, options.DbContextName));
        model.Files.Add(new MinimalApiProgramFileModel(model.Namespace, model.Directory, model.Namespace, options.DbContextName, entities));

        model.Packages.Add(new() { Name = "Microsoft.EntityFrameworkCore.InMemory", Version = "6.0.2" });
        model.Packages.Add(new() { Name = "Swashbuckle.AspNetCore.Annotations", Version = "6.2.3" });
        model.Packages.Add(new() { Name = "Swashbuckle.AspNetCore.Newtonsoft", Version = "6.2.3" });
        model.Packages.Add(new() { Name = "MinimalApis.Extensions", IsPreRelease = true });

        return model;
    }

    public async Task<ProjectModel> CreateMinimalApiUnitTestsProject(string name, string directory, string resource)
    {
        var model = new ProjectModel(DotNetProjectType.XUnit, $"{name}.Tests", directory);

        return model;
    }

    public async Task<ProjectModel> CreateLibrary(string name, string parentDirectory, List<string> additionalMetadata = null)
    {
        var project = new ProjectModel(name, parentDirectory);

        var serviceName = name.Split('.').First();

        if (additionalMetadata != null)
        {
            foreach (var m in additionalMetadata)
            {
                project.Metadata.Add(m);
            }
        }


        foreach (var metadataItem in project.Metadata)
        {
            switch (metadataItem)
            {
                case Constants.ProjectType.Core:

                    project.DotNetProjectType = DotNetProjectType.ClassLib;

                    project.Folders.Add(new("AggregatesModel", project.Directory));

                    project.Files.Add(_fileFactory.CreateResponseBase(project.Directory));
                    project.Files.Add(_fileFactory.CreateCoreUsings(project.Directory));
                    project.Files.Add(_fileFactory.CreateLinqExtensions(project.Directory));
                    project.Files.Add(_fileFactory.CreateTemplate("DddApp.Core.ConfigureServices", "ConfigureServices", project.Directory, ".cs", tokens: new TokensBuilder().With("serviceName", serviceName).Build()));

                    project.Packages.Add(new("FluentValidation", "11.5.1"));
                    project.Packages.Add(new("FluentValidation.DependencyInjectionExtensions", "11.5.1"));
                    project.Packages.Add(new("MediatR", "12.0.0"));
                    project.Packages.Add(new("Newtonsoft.Json", "13.0.2"));
                    project.Packages.Add(new("Microsoft.EntityFrameworkCore", "7.0.2"));
                    project.Packages.Add(new("Microsoft.Extensions.Hosting.Abstractions", "7.0.0"));
                    project.Packages.Add(new("Microsoft.Extensions.Logging.Abstractions", "7.0.0"));
                    project.Packages.Add(new("SerilogTimings", "3.0.1"));
                    project.Packages.Add(new("Microsoft.AspNetCore.Authentication.JwtBearer", "6.5.0"));
                    project.Packages.Add(new("System.IdentityModel.Tokens.Jwt", "6.25.1"));

                    project.References.Add(@"..\BuildingBlocks\Kernel\Kernel.csproj");

                    break;

                case Constants.ProjectType.Domain:
                    project.Packages.Add(new("FluentValidation", "11.4.0"));
                    project.Packages.Add(new("MediatR", "12.0.0"));
                    project.Packages.Add(new("Newtonsoft.Json", "13.0.2"));
                    project.Packages.Add(new("Microsoft.EntityFrameworkCore", "7.0.2"));
                    break;

                case Constants.ProjectType.Application:
                    break;

                case Constants.ProjectType.Infrastructure:

                    var tokens = new TokensBuilder()
                        .With("serviceName", serviceName)
                        .Build();

                    project.DotNetProjectType = DotNetProjectType.ClassLib;
                    project.Folders.Add(new("Data", project.Directory));
                    project.Files.Add(_fileFactory.CreateTemplate("DddApp.Infrastructure.ConfigureServices", "ConfigureServices", project.Directory, ".cs", tokens: tokens));

                    project.Files.Add(_fileFactory.CreateTemplate("DddApp.Infrastructure.SeedData", "SeedData", Path.Combine(project.Directory, "Data"), ".cs", tokens: new TokensBuilder()
                        .With("serviceName", serviceName)
                        .With("namespace", $"{serviceName}.Infrastructure.Data")
                        .Build()));

                    project.Files.Add(_fileFactory.CreateTemplate("DddApp.Infrastructure.DesignTimeDbContextFactory", "DesignTimeDbContextFactory", project.Directory, ".cs", tokens: tokens));

                    project.References.Add($"..{Path.DirectorySeparatorChar}{serviceName}.Core{Path.DirectorySeparatorChar}{serviceName}.Core.csproj");

                    project.Packages.Add(new("Microsoft.EntityFrameworkCore.Tools", "7.0.0"));
                    project.Packages.Add(new("Microsoft.EntityFrameworkCore.SqlServer", "7.0.2"));
                    project.Packages.Add(new("Microsoft.EntityFrameworkCore.Design", "7.0.2"));
                    break;

                case Constants.ProjectType.Api:

                    var schema = serviceName.Remove("Service");

                    project.DotNetProjectType = DotNetProjectType.WebApi;

                    project.Folders.Add(new("Controllers", project.Directory));

                    project.References.Add($"..{Path.DirectorySeparatorChar}{serviceName}.Infrastructure{Path.DirectorySeparatorChar}{serviceName}.Infrastructure.csproj");

                    project.Files.Add(_fileFactory.CreateTemplate("DddApp.Api.AppSettings", "appsettings", project.Directory, ".json", tokens: new TokensBuilder().With("serviceName", serviceName).Build()));

                    project.Files.Add(_fileFactory.CreateTemplate("Api.ConfigureServices", "ConfigureServices", project.Directory, tokens: new TokensBuilder()
                        .With("DbContext", $"{serviceName}DbContext")
                        .With("serviceName", serviceName)
                        .Build()));

                    project.Files.Add(_fileFactory.CreateTemplate("Api.Program", "Program", project.Directory, tokens: new TokensBuilder()
                        .With("DbContext", $"{serviceName}DbContext")
                        .With("serviceName", serviceName)
                        .With("schema", schema)
                        .Build()));

                    project.Packages.Add(new("Microsoft.AspNetCore.Mvc.Versioning", "5.0.0"));
                    project.Packages.Add(new() { Name = "Microsoft.AspNetCore.OpenApi", Version = "7.0.2" });
                    project.Packages.Add(new("Serilog", "2.12.0"));
                    project.Packages.Add(new("SerilogTimings", "3.0.1"));
                    project.Packages.Add(new("Serilog.AspNetCore", "6.0.1"));
                    project.Packages.Add(new("Swashbuckle.AspNetCore", "6.5.0"));
                    project.Packages.Add(new("Swashbuckle.AspNetCore.Annotations", "6.5.0"));
                    project.Packages.Add(new("Swashbuckle.AspNetCore.Newtonsoft", "6.5.0"));
                    project.Packages.Add(new("Swashbuckle.AspNetCore.Swagger", "6.5.0"));
                    break;

            }
        }
        return project;
    }

    public async Task<ProjectModel> CreateWebApi(string name, string parentDirectory, List<string> additionalMetadata = null)
    {
        var project = new ProjectModel(name, parentDirectory)
        {
            DotNetProjectType = DotNetProjectType.WebApi
        };

        if (additionalMetadata != null)
            project.Metadata.Concat(additionalMetadata);

        return project;
    }

    public async Task<ProjectModel> CreateTestingProject()
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectModel> CreateUnitTestsProject()
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectModel> CreateIntegrationTestsProject()
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectModel> CreateMessagingProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Messaging", directory);

        model.Files.Add(_fileFactory.CreateTemplate("IMessagingClient", "IMessagingClient", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("IServiceBusMessage", "IServiceBusMessage", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("IServiceBusMessageListener", "IServiceBusMessageListener", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("IServiceBusMessageSender", "IServiceBusMessageSender", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("ReceiveRequest", "ReceiveRequest", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("ServiceBusMessage", "ServiceBusMessage", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("Observable", "Observable", $"{model.Directory}{Path.DirectorySeparatorChar}Internals"));

        model.Packages.Add(new() { Name = "MediatR.Contracts", Version = "1.0.1" });

        model.Packages.Add(new() { Name = "Newtonsoft.Json", Version = "13.0.2" });

        return model;
    }

    public async Task<ProjectModel> CreateMessagingUdpProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Messaging.Udp", directory);

        model.References.Add(@"..\Messaging\Messaging.csproj");

        model.Packages.Add(new() { Name = "Microsoft.Extensions.DependencyInjection.Abstractions", Version = "7.0.0" });

        model.Packages.Add(new() { Name = "Microsoft.Extensions.Logging.Abstractions", Version = "7.0.0" });

        model.Packages.Add(new() { Name = "System.Reactive.Linq", Version = "5.0.0" });

        model.Files.Add(_fileFactory.CreateTemplate("Messaging.Udp.ConfigureServices", "ConfigureServices", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("IUdpClientFactory", "IUdpClientFactory", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("MessagingClient", "MessagingClient", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("ServiceBusMessageListener", "ServiceBusMessageListener", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("ServiceBusMessageSender", "ServiceBusMessageSender", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("UdpClientFactory", "UdpClientFactory", model.Directory));

        return model;
    }

    public async Task<ProjectModel> CreateValidationProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Validation", directory);



        return model;
    }

    public async Task<ProjectModel> CreateKernelProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Kernel", directory);

        model.Packages.Add(new("Microsoft.EntityFrameworkCore", "7.0.2"));
        model.Packages.Add(new("Microsoft.AspNetCore.Mvc.Core", "2.2.5"));
        model.Packages.Add(new("Microsoft.AspNetCore.SignalR.Core", "1.1.0"));
        model.Packages.Add(new("Microsoft.EntityFrameworkCore", "7.0.2"));
        model.Packages.Add(new("Microsoft.Extensions.Hosting.Abstractions", "7.0.0"));

        var responseBase = new ClassModel("ResponseBase");

        var responseBaseConstructor = new ConstructorModel(responseBase, responseBase.Name)
        {
            Body = "Errors = new List<string>();"
        };

        responseBase.Properties.Add(new(responseBase, AccessModifier.Public, TypeModel.ListOf("string"), "Errors", PropertyAccessorModel.GetPrivateSet));

        responseBase.Constructors.Add(responseBaseConstructor);

        var entityFrameworFileModel = _fileFactory.CreateTemplate("BuildingBlocks.Kernel.EntityFrameworkCoreExtensions", "EntityFrameworkCoreExtensions", model.Directory);

        var globalExceptionFilterFileModel = _fileFactory.CreateTemplate("BuildingBlocks.Kernel.HttpGlobalExceptionFilter", "HttpGlobalExceptionFilter", model.Directory);

        var domainExceptionfileModel = _fileFactory.CreateTemplate("BuildingBlocks.Kernel.DomainException", "DomainException", model.Directory);

        var internalServerfileModel = _fileFactory.CreateTemplate("BuildingBlocks.Kernel.InternalServerErrorObjectResult", "InternalServerErrorObjectResult", model.Directory);

        var jsonResponsefileModel = _fileFactory.CreateTemplate("BuildingBlocks.Kernel.JsonErrorResponse", "JsonErrorResponse", model.Directory);

        model.Files.Add(entityFrameworFileModel);

        model.Files.Add(domainExceptionfileModel);

        model.Files.Add(internalServerfileModel);

        model.Files.Add(globalExceptionFilterFileModel);

        model.Files.Add(jsonResponsefileModel);

        model.Files.Add(new ObjectFileModel<ClassModel>(responseBase, responseBase.UsingDirectives, responseBase.Name, model.Directory, ".cs"));

        return model;
    }

    public async Task<ProjectModel> CreateSecurityProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Security", directory);

        model.Packages.Add(new() { Name = "MediatR", Version = "12.0.0" });
        model.Packages.Add(new() { Name = "Microsoft.AspNetCore.Authentication.JwtBearer", Version = "7.0.2" });
        model.Packages.Add(new() { Name = "Swashbuckle.AspNetCore.SwaggerGen", Version = "6.5.0" });
        model.Packages.Add(new() { Name = "System.IdentityModel.Tokens.Jwt", Version = "6.25.1" });

        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.AccessRight", "AccessRight", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.Authentication", "Authentication", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.AuthorizationHeaderParameterOperationFilter", "AuthorizationHeaderParameterOperationFilter", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.AuthorizeResourceOperationAttribute", "AuthorizeResourceOperationAttribute", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.ConfigureServices", "ConfigureServices", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.IPasswordHasher", "IPasswordHasher", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.ITokenBuilder", "ITokenBuilder", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.ITokenProvider", "ITokenProvider", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.Operations", "Operations", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.PasswordHasher", "PasswordHasher", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.ResourceOperationAuthorizationBehavior", "ResourceOperationAuthorizationBehavior", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.ResourceOperationAuthorizationHandler", "ResourceOperationAuthorizationHandler", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.SecurityConstants", "SecurityConstants", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.TokenBuilder", "TokenBuilder", model.Directory));
        model.Files.Add(_fileFactory.CreateTemplate("BuildingBlocks.Security.Security.TokenProvider", "TokenProvider", model.Directory));

        return model;
    }

    public async Task<ProjectModel> Create(string type, string name, string directory, List<string> references = null, string metadata = null)
    {
        var model = await CreateLibrary(name, directory, metadata?.Split(',').ToList());

        model.DotNetProjectType = type switch
        {
            "web" => DotNetProjectType.Web,
            "webapi" => DotNetProjectType.WebApi,
            "classlib" => DotNetProjectType.ClassLib,
            "worker" => DotNetProjectType.Worker,
            "xunit" => DotNetProjectType.XUnit,
            _ => DotNetProjectType.Console
        };

        var parts = name.Split('.');

        var layer = parts.Length > 1 ? parts.Last() : name;

        var tokens = new TokensBuilder()
                .With("Layer", layer)
                .Build();


        if (type.ToLower() == "benchmark")
        {
            model.Packages.Add(new() { Name = "BenchmarkDotNet", Version = "0.13.6" });
        }

        if (type.StartsWith("web"))
        {
            model.Files.Add(_fileFactory.CreateTemplate("DefaultProgram", "Program", model.Directory, tokens: tokens));

            model.Files.Add(_fileFactory.CreateTemplate("DefaultConfigureServices", "ConfigureServices", model.Directory, tokens: tokens));

            model.Packages.Add(new() { Name = "Microsoft.AspNetCore.OpenApi", Version = "7.0.2" });

            model.Packages.Add(new() { Name = "Swashbuckle.AspNetCore", Version = "6.4.0" });
        }

        return model;
    }

    public async Task<ProjectModel> CreateCore(string name, string directory)
        => await CreateLibrary($"{name}.Core", directory, new() { Constants.ProjectType.Core });

    public async Task<ProjectModel> CreateInfrastructure(string name, string directory)
        => await CreateLibrary($"{name}.Infrastructure", directory, new() { Constants.ProjectType.Infrastructure });

    public async Task<ProjectModel> CreateApi(string name, string directory)
        => await CreateLibrary($"{name}.Api", directory, new() { Constants.ProjectType.Api });

    public async Task<ProjectModel> CreateIOCompression(string directory)
    {
        var model = new ProjectModel("IO.Compression", directory);

        model.Files.Add(_fileFactory.CreateTemplate("IPackable", "IPackable", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("BitVector8", "BitVector8", model.Directory));

        model.Files.Add(_fileFactory.CreateTemplate("String255Type", "String255Type", Path.Combine(model.Directory, "Primitives")));

        model.Files.Add(_fileFactory.CreateTemplate("Int32Type", "Int32Type", Path.Combine(model.Directory, "Primitives")));

        model.Files.Add(_fileFactory.CreateTemplate("Int16Type", "Int16Type", Path.Combine(model.Directory, "Primitives")));

        model.Files.Add(_fileFactory.CreateTemplate("GuidType", "GuidType", Path.Combine(model.Directory, "Primitives")));

        model.Files.Add(_fileFactory.CreateTemplate("BoolType", "BoolType", Path.Combine(model.Directory, "Primitives")));

        return model;
    }

    public async Task<ProjectModel> CreateCommon(string directory)
    {
        var model = new ProjectModel("Services.Common", directory);

        return model;
    }
}