// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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

namespace Endpoint.Core.Artifacts.Projects.Factories;

public class ProjectFactory : IProjectFactory
{
    private readonly IFileFactory fileFactory;
    private readonly ILogger<ProjectFactory> logger;

    public ProjectFactory(IFileFactory fileFactory, ILogger<ProjectFactory> logger)
    {
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ProjectModel> CreateSpecFlowProject(string name, string directory)
    {
        var model = await CreateLibrary(name, directory);

        model.DotNetProjectType = DotNetProjectType.XUnit;

        model.Packages.Add(new () { Name = "SpecFlow.XUnit" });
        model.Packages.Add(new () { Name = "Microsoft.Extensions.Configuration.Json" });
        model.Packages.Add(new () { Name = "FluentAssertions" });
        model.Packages.Add(new () { Name = "Ductus.FluentDocker" });

        return model;
    }

    public async Task<ProjectModel> CreatePlaywrightProject(string name, string directory)
    {
        var model = await CreateLibrary(name, directory);

        model.DotNetProjectType = DotNetProjectType.NUnit;

        model.Packages.Add(new () { Name = "Microsoft.Playwright.NUnit" });
        model.Packages.Add(new () { Name = "SpecFlow.NUnit" });

        return model;
    }

    public async Task<ProjectModel> CreateHttpProject(string name, string directory)
    {
        var model = new ProjectModel(DotNetProjectType.Console, name, directory);

        model.Packages.Add(new ("StyleCop.Analyzers", "1.1.118"));

        model.Files.Add(fileFactory.CreateCSharp("EmptyProgram", string.Empty, "Program", model.Directory));
        model.Files.Add(fileFactory.CreateCSharp("HttpClientExtensions", string.Empty, "HttpClientExtensions", model.Directory));
        model.Files.Add(fileFactory.CreateCSharp("HttpClientFactory", string.Empty, "HttpClientFactory", model.Directory));

        return model;
    }

    public async Task<ProjectModel> CreateMinimalApiProject(CreateMinimalApiProjectOptions options)
    {
        var model = new ProjectModel(DotNetProjectType.MinimalWebApi, options.Name, options.Directory);

        model.Packages.Add(new ("StyleCop.Analyzers", "1.1.118"));

        var entities = new List<EntityModel> { new EntityModel(options.Resource) };

        model.GenerateDocumentationFile = true;

        model.Files.Add(fileFactory.LaunchSettingsJson(model.Directory, model.Name, options.Port.Value));
        model.Files.Add(fileFactory.AppSettings(model.Directory, model.Name, options.DbContextName));
        model.Files.Add(new MinimalApiProgramFileModel(model.Namespace, model.Directory, model.Namespace, options.DbContextName, entities));

        model.Packages.Add(new () { Name = "Microsoft.EntityFrameworkCore.InMemory", Version = "6.0.2" });
        model.Packages.Add(new () { Name = "Swashbuckle.AspNetCore.Annotations", Version = "6.2.3" });
        model.Packages.Add(new () { Name = "Swashbuckle.AspNetCore.Newtonsoft", Version = "6.2.3" });
        model.Packages.Add(new () { Name = "MinimalApis.Extensions", IsPreRelease = true });

        return model;
    }

    public async Task<ProjectModel> CreateMinimalApiUnitTestsProject(string name, string directory, string resource)
    {
        var model = new ProjectModel(DotNetProjectType.XUnit, $"{name}.Tests", directory);

        model.Packages.Add(new ("StyleCop.Analyzers", "1.1.118"));

        return model;
    }

    public async Task<ProjectModel> CreateLibrary(string name, string parentDirectory, List<string> additionalMetadata = null)
    {
        var model = new ProjectModel(name, parentDirectory);

        model.Packages.Add(new ("StyleCop.Analyzers", "1.1.118"));

        var serviceName = name.Split('.').First();

        if (additionalMetadata != null)
        {
            foreach (var m in additionalMetadata)
            {
                model.Metadata.Add(m);
            }
        }

        foreach (var metadataItem in model.Metadata)
        {
            switch (metadataItem)
            {
                case Constants.ProjectType.Core:

                    model.DotNetProjectType = DotNetProjectType.ClassLib;

                    model.Folders.Add(new ("AggregatesModel", model.Directory));

                    model.Files.Add(fileFactory.CreateResponseBase(model.Directory));
                    model.Files.Add(fileFactory.CreateCoreUsings(model.Directory));
                    model.Files.Add(fileFactory.CreateLinqExtensions(model.Directory));
                    model.Files.Add(fileFactory.CreateTemplate("DddApp.Core.ConfigureServices", "ConfigureServices", model.Directory, ".cs", tokens: new TokensBuilder().With("serviceName", serviceName).Build()));

                    model.Packages.Add(new ("FluentValidation", "11.5.1"));
                    model.Packages.Add(new ("FluentValidation.DependencyInjectionExtensions", "11.5.1"));
                    model.Packages.Add(new ("MediatR", "12.0.0"));
                    model.Packages.Add(new ("Newtonsoft.Json", "13.0.2"));
                    model.Packages.Add(new ("Microsoft.EntityFrameworkCore", "7.0.2"));
                    model.Packages.Add(new ("Microsoft.Extensions.Hosting.Abstractions", "7.0.0"));
                    model.Packages.Add(new ("Microsoft.Extensions.Logging.Abstractions", "7.0.0"));
                    model.Packages.Add(new ("SerilogTimings", "3.0.1"));
                    model.Packages.Add(new ("Microsoft.AspNetCore.Authentication.JwtBearer", "6.5.0"));
                    model.Packages.Add(new ("System.IdentityModel.Tokens.Jwt", "6.25.1"));

                    model.References.Add(@"..\BuildingBlocks\Kernel\Kernel.csproj");

                    break;

                case Constants.ProjectType.Domain:
                    model.Packages.Add(new ("FluentValidation", "11.4.0"));
                    model.Packages.Add(new ("MediatR", "12.0.0"));
                    model.Packages.Add(new ("Newtonsoft.Json", "13.0.2"));
                    model.Packages.Add(new ("Microsoft.EntityFrameworkCore", "7.0.2"));
                    break;

                case Constants.ProjectType.Application:
                    break;

                case Constants.ProjectType.Infrastructure:

                    var tokens = new TokensBuilder()
                        .With("serviceName", serviceName)
                        .Build();

                    model.DotNetProjectType = DotNetProjectType.ClassLib;
                    model.Folders.Add(new ("Data", model.Directory));
                    model.Files.Add(fileFactory.CreateTemplate("DddApp.Infrastructure.ConfigureServices", "ConfigureServices", model.Directory, ".cs", tokens: tokens));

                    model.Files.Add(fileFactory.CreateTemplate("DddApp.Infrastructure.SeedData", "SeedData", Path.Combine(model.Directory, "Data"), ".cs", tokens: new TokensBuilder()
                        .With("serviceName", serviceName)
                        .With("namespace", $"{serviceName}.Infrastructure.Data")
                        .Build()));

                    model.Files.Add(fileFactory.CreateTemplate("DddApp.Infrastructure.DesignTimeDbContextFactory", "DesignTimeDbContextFactory", model.Directory, ".cs", tokens: tokens));

                    model.References.Add($"..{Path.DirectorySeparatorChar}{serviceName}.Core{Path.DirectorySeparatorChar}{serviceName}.Core.csproj");

                    model.Packages.Add(new ("Microsoft.EntityFrameworkCore.Tools", "7.0.0"));
                    model.Packages.Add(new ("Microsoft.EntityFrameworkCore.SqlServer", "7.0.2"));
                    model.Packages.Add(new ("Microsoft.EntityFrameworkCore.Design", "7.0.2"));
                    break;

                case Constants.ProjectType.Api:

                    var schema = serviceName.Remove("Service");

                    model.DotNetProjectType = DotNetProjectType.WebApi;

                    model.Folders.Add(new ("Controllers", model.Directory));

                    model.References.Add($"..{Path.DirectorySeparatorChar}{serviceName}.Infrastructure{Path.DirectorySeparatorChar}{serviceName}.Infrastructure.csproj");

                    model.Files.Add(fileFactory.CreateTemplate("DddApp.Api.AppSettings", "appsettings", model.Directory, ".json", tokens: new TokensBuilder().With("serviceName", serviceName).Build()));

                    model.Files.Add(fileFactory.CreateTemplate("Api.ConfigureServices", "ConfigureServices", model.Directory, tokens: new TokensBuilder()
                        .With("DbContext", $"{serviceName}DbContext")
                        .With("serviceName", serviceName)
                        .Build()));

                    model.Files.Add(fileFactory.CreateTemplate("Api.Program", "Program", model.Directory, tokens: new TokensBuilder()
                        .With("DbContext", $"{serviceName}DbContext")
                        .With("serviceName", serviceName)
                        .With("schema", schema)
                        .Build()));

                    model.Packages.Add(new ("Microsoft.AspNetCore.Mvc.Versioning", "5.0.0"));
                    model.Packages.Add(new () { Name = "Microsoft.AspNetCore.OpenApi", Version = "7.0.2" });
                    model.Packages.Add(new ("Serilog", "2.12.0"));
                    model.Packages.Add(new ("SerilogTimings", "3.0.1"));
                    model.Packages.Add(new ("Serilog.AspNetCore", "6.0.1"));
                    model.Packages.Add(new ("Swashbuckle.AspNetCore", "6.5.0"));
                    model.Packages.Add(new ("Swashbuckle.AspNetCore.Annotations", "6.5.0"));
                    model.Packages.Add(new ("Swashbuckle.AspNetCore.Newtonsoft", "6.5.0"));
                    model.Packages.Add(new ("Swashbuckle.AspNetCore.Swagger", "6.5.0"));
                    break;
            }
        }

        return model;
    }

    public async Task<ProjectModel> CreateWebApi(string name, string parentDirectory, List<string> additionalMetadata = null)
    {
        var model = new ProjectModel(name, parentDirectory)
        {
            DotNetProjectType = DotNetProjectType.WebApi,
        };

        model.Packages.Add(new ("StyleCop.Analyzers", "1.1.118"));

        if (additionalMetadata != null)
        {
            model.Metadata.Concat(additionalMetadata);
        }

        return model;
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

        model.Packages.Add(new ("StyleCop.Analyzers", "1.1.118"));

        model.Files.Add(fileFactory.CreateTemplate("IMessagingClient", "IMessagingClient", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("IServiceBusMessage", "IServiceBusMessage", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("IServiceBusMessageListener", "IServiceBusMessageListener", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("IServiceBusMessageSender", "IServiceBusMessageSender", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("ReceiveRequest", "ReceiveRequest", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("ServiceBusMessage", "ServiceBusMessage", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("Observable", "Observable", $"{model.Directory}{Path.DirectorySeparatorChar}Internals"));

        model.Packages.Add(new () { Name = "MediatR.Contracts", Version = "1.0.1" });

        model.Packages.Add(new () { Name = "Newtonsoft.Json", Version = "13.0.2" });

        return model;
    }

    public async Task<ProjectModel> CreateMessagingUdpProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Messaging.Udp", directory);

        model.Packages.Add(new ("StyleCop.Analyzers", "1.1.118"));

        model.References.Add(@"..\Messaging\Messaging.csproj");

        model.Packages.Add(new () { Name = "Microsoft.Extensions.DependencyInjection.Abstractions", Version = "7.0.0" });

        model.Packages.Add(new () { Name = "Microsoft.Extensions.Logging.Abstractions", Version = "7.0.0" });

        model.Packages.Add(new () { Name = "System.Reactive.Linq", Version = "5.0.0" });

        model.Files.Add(fileFactory.CreateTemplate("Messaging.Udp.ConfigureServices", "ConfigureServices", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("IUdpClientFactory", "IUdpClientFactory", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("MessagingClient", "MessagingClient", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("ServiceBusMessageListener", "ServiceBusMessageListener", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("ServiceBusMessageSender", "ServiceBusMessageSender", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("UdpClientFactory", "UdpClientFactory", model.Directory));

        return model;
    }

    public async Task<ProjectModel> CreateValidationProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Validation", directory);

        model.Packages.Add(new ("StyleCop.Analyzers", "1.1.118"));

        return model;
    }

    public async Task<ProjectModel> CreateKernelProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Kernel", directory);

        model.Packages.Add(new ("StyleCop.Analyzers", "1.1.118"));

        model.Packages.Add(new ("Microsoft.EntityFrameworkCore", "7.0.2"));
        model.Packages.Add(new ("Microsoft.AspNetCore.Mvc.Core", "2.2.5"));
        model.Packages.Add(new ("Microsoft.AspNetCore.SignalR.Core", "1.1.0"));
        model.Packages.Add(new ("Microsoft.EntityFrameworkCore", "7.0.2"));
        model.Packages.Add(new ("Microsoft.Extensions.Hosting.Abstractions", "7.0.0"));

        var responseBase = new ClassModel("ResponseBase");

        var responseBaseConstructor = new ConstructorModel(responseBase, responseBase.Name)
        {
            Body = new ("Errors = new List<string>();"),
        };

        responseBase.Properties.Add(new (responseBase, AccessModifier.Public, TypeModel.ListOf("string"), "Errors", PropertyAccessorModel.GetPrivateSet));

        responseBase.Constructors.Add(responseBaseConstructor);

        var entityFrameworFileModel = fileFactory.CreateTemplate("BuildingBlocks.Kernel.EntityFrameworkCoreExtensions", "EntityFrameworkCoreExtensions", model.Directory);

        var globalExceptionFilterFileModel = fileFactory.CreateTemplate("BuildingBlocks.Kernel.HttpGlobalExceptionFilter", "HttpGlobalExceptionFilter", model.Directory);

        var domainExceptionfileModel = fileFactory.CreateTemplate("BuildingBlocks.Kernel.DomainException", "DomainException", model.Directory);

        var internalServerfileModel = fileFactory.CreateTemplate("BuildingBlocks.Kernel.InternalServerErrorObjectResult", "InternalServerErrorObjectResult", model.Directory);

        var jsonResponsefileModel = fileFactory.CreateTemplate("BuildingBlocks.Kernel.JsonErrorResponse", "JsonErrorResponse", model.Directory);

        model.Files.Add(entityFrameworFileModel);

        model.Files.Add(domainExceptionfileModel);

        model.Files.Add(internalServerfileModel);

        model.Files.Add(globalExceptionFilterFileModel);

        model.Files.Add(jsonResponsefileModel);

        model.Files.Add(new CodeFileModel<ClassModel>(responseBase, responseBase.Usings, responseBase.Name, model.Directory, ".cs"));

        return model;
    }

    public async Task<ProjectModel> CreateSecurityProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Security", directory);

        model.Packages.Add(new () { Name = "MediatR", Version = "12.0.0" });
        model.Packages.Add(new () { Name = "Microsoft.AspNetCore.Authentication.JwtBearer", Version = "7.0.2" });
        model.Packages.Add(new () { Name = "Swashbuckle.AspNetCore.SwaggerGen", Version = "6.5.0" });
        model.Packages.Add(new () { Name = "System.IdentityModel.Tokens.Jwt", Version = "6.25.1" });

        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.AccessRight", "AccessRight", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.Authentication", "Authentication", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.AuthorizationHeaderParameterOperationFilter", "AuthorizationHeaderParameterOperationFilter", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.AuthorizeResourceOperationAttribute", "AuthorizeResourceOperationAttribute", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.ConfigureServices", "ConfigureServices", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.IPasswordHasher", "IPasswordHasher", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.ITokenBuilder", "ITokenBuilder", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.ITokenProvider", "ITokenProvider", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.Operations", "Operations", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.PasswordHasher", "PasswordHasher", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.ResourceOperationAuthorizationBehavior", "ResourceOperationAuthorizationBehavior", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.ResourceOperationAuthorizationHandler", "ResourceOperationAuthorizationHandler", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.SecurityConstants", "SecurityConstants", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.TokenBuilder", "TokenBuilder", model.Directory));
        model.Files.Add(fileFactory.CreateTemplate("BuildingBlocks.Security.Security.TokenProvider", "TokenProvider", model.Directory));

        return model;
    }

    public async Task<ProjectModel> Create(string type, string name, string directory, List<string> references = null, string metadata = null)
    {
        var dotNetProjectType = type switch
        {
            "web" => DotNetProjectType.Web,
            "webapi" => DotNetProjectType.WebApi,
            "classlib" => DotNetProjectType.ClassLib,
            "worker" => DotNetProjectType.Worker,
            "xunit" => DotNetProjectType.XUnit,
            "ts" => DotNetProjectType.TypeScriptStandalone,
            _ => DotNetProjectType.Console
        };

        var model = new ProjectModel(dotNetProjectType, name, directory);

        var parts = name.Split('.');

        var layer = parts.Length > 1 ? parts.Last() : name;

        var tokens = new TokensBuilder()
                .With("Layer", layer)
                .Build();

        if (type.ToLower() == "benchmark")
        {
            model.Packages.Add(new () { Name = "BenchmarkDotNet", Version = "0.13.6" });
        }

        if (type.StartsWith("web"))
        {
            model.Files.Add(fileFactory.CreateTemplate("DefaultProgram", "Program", model.Directory, tokens: tokens));

            model.Files.Add(fileFactory.CreateTemplate("DefaultConfigureServices", "ConfigureServices", model.Directory, tokens: tokens));

            model.Packages.Add(new () { Name = "Microsoft.AspNetCore.OpenApi", Version = "7.0.2" });

            model.Packages.Add(new () { Name = "Swashbuckle.AspNetCore", Version = "6.4.0" });
        }

        return model;
    }

    public async Task<ProjectModel> CreateCore(string name, string directory)
        => await CreateLibrary($"{name}.Core", directory, new () { Constants.ProjectType.Core });

    public async Task<ProjectModel> CreateInfrastructure(string name, string directory)
        => await CreateLibrary($"{name}.Infrastructure", directory, new () { Constants.ProjectType.Infrastructure });

    public async Task<ProjectModel> CreateApi(string name, string directory)
        => await CreateLibrary($"{name}.Api", directory, new () { Constants.ProjectType.Api });

    public async Task<ProjectModel> CreateIOCompression(string directory)
    {
        var model = new ProjectModel("IO.Compression", directory);

        model.Files.Add(fileFactory.CreateTemplate("IPackable", "IPackable", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("BitVector8", "BitVector8", model.Directory));

        model.Files.Add(fileFactory.CreateTemplate("String255Type", "String255Type", Path.Combine(model.Directory, "Primitives")));

        model.Files.Add(fileFactory.CreateTemplate("Int32Type", "Int32Type", Path.Combine(model.Directory, "Primitives")));

        model.Files.Add(fileFactory.CreateTemplate("Int16Type", "Int16Type", Path.Combine(model.Directory, "Primitives")));

        model.Files.Add(fileFactory.CreateTemplate("GuidType", "GuidType", Path.Combine(model.Directory, "Primitives")));

        model.Files.Add(fileFactory.CreateTemplate("BoolType", "BoolType", Path.Combine(model.Directory, "Primitives")));

        return model;
    }

    public async Task<ProjectModel> CreateCommon(string directory)
    {
        var model = new ProjectModel("Services.Common", directory);

        return model;
    }
}