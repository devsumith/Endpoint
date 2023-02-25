// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Enums;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Artifacts.Folders;
using Endpoint.Core.Models.Artifacts.Projects.Enums;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Projects.Factories;

public class ProjectModelFactory : IProjectModelFactory
{
    private readonly IFileModelFactory _fileModelFactory;
    private readonly INamespaceProvider _namespaceProvider;

    public ProjectModelFactory(IFileModelFactory fileModelFactory, INamespaceProvider namespaceProvider)
    {
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
    }

    public ProjectModel CreateSpecFlowProject(string name, string directory)
    {
        var model = CreateLibrary(name, directory);

        model.DotNetProjectType = DotNetProjectType.XUnit;

        model.Packages.Add(new PackageModel() { Name = "SpecFlow.XUnit" });

        model.Packages.Add(new PackageModel() { Name = "Microsoft.Extensions.Configuration.Json" });

        model.Packages.Add(new PackageModel() { Name = "FluentAssertions" });

        model.Packages.Add(new PackageModel() { Name = "Ductus.FluentDocker" });

        return model;
    }

    public ProjectModel CreatePlaywrightProject(string name, string directory)
    {
        var model = CreateLibrary(name, directory);

        model.DotNetProjectType = DotNetProjectType.NUnit;

        model.Packages.Add(new PackageModel() { Name = "Microsoft.Playwright.NUnit" });

        model.Packages.Add(new PackageModel() { Name = "SpecFlow.NUnit" });

        return model;
    }

    public ProjectModel CreateHttpProject(string name, string directory)
    {
        var model = new ProjectModel(DotNetProjectType.Console, name, directory);

        model.Files.Add(_fileModelFactory.CreateCSharp("EmptyProgram", "", "Program", model.Directory));

        model.Files.Add(_fileModelFactory.CreateCSharp("HttpClientExtensions", "", "HttpClientExtensions", model.Directory));

        model.Files.Add(_fileModelFactory.CreateCSharp("HttpClientFactory", "", "HttpClientFactory", model.Directory));

        return model;
    }

    public ProjectModel CreateMinimalApiProject(CreateMinimalApiProjectOptions options)
    {
        var model = new ProjectModel(DotNetProjectType.MinimalWebApi, options.Name, options.Directory);

        var entities = new List<EntityModel> { new EntityModel(options.Resource) };

        model.GenerateDocumentationFile = true;

        model.Files.Add(_fileModelFactory.LaunchSettingsJson(model.Directory, model.Name, options.Port.Value));

        model.Files.Add(_fileModelFactory.AppSettings(model.Directory, model.Name, options.DbContextName));

        model.Files.Add(new MinimalApiProgramFileModel(model.Namespace, model.Directory, model.Namespace, options.DbContextName, entities));

        model.Packages.Add(new() { Name = "Microsoft.EntityFrameworkCore.InMemory", Version = "6.0.2" });

        model.Packages.Add(new() { Name = "Swashbuckle.AspNetCore.Annotations", Version = "6.2.3" });

        model.Packages.Add(new() { Name = "Swashbuckle.AspNetCore.Newtonsoft", Version = "6.2.3" });

        model.Packages.Add(new() { Name = "MinimalApis.Extensions", IsPreRelease = true });

        return model;
    }

    public ProjectModel CreateMinimalApiUnitTestsProject(string name, string directory, string resource)
    {
        var model = new ProjectModel(DotNetProjectType.XUnit, $"{name}.Tests", directory);

        return model;
    }

    public ProjectModel CreateLibrary(string name, string parentDirectory, List<string> additionalMetadata = null)
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

                    project.Folders.Add(new FolderModel("AggregateModel", project.Directory));

                    project.Files.Add(_fileModelFactory.CreateResponseBase(project.Directory));
                    project.Files.Add(_fileModelFactory.CreateCoreUsings(project.Directory));
                    project.Files.Add(_fileModelFactory.CreateLinqExtensions(project.Directory));
                    project.Files.Add(_fileModelFactory.CreateTemplate("DddApp.Core.ConfigureServices", "ConfigureServices", project.Directory, "cs", tokens: new TokensBuilder().With("serviceName", serviceName).Build()));

                    project.Packages.Add(new PackageModel("FluentValidation", "11.5.1"));
                    project.Packages.Add(new PackageModel("FluentValidation.DependencyInjectionExtensions", "11.5.1"));
                    project.Packages.Add(new PackageModel("MediatR", "12.0.0"));
                    project.Packages.Add(new PackageModel("Newtonsoft.Json", "13.0.2"));
                    project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore", "7.0.2"));
                    project.Packages.Add(new PackageModel("Microsoft.Extensions.Hosting.Abstractions", "7.0.0"));
                    project.Packages.Add(new PackageModel("Microsoft.Extensions.Logging.Abstractions", "7.0.0"));
                    project.Packages.Add(new PackageModel("SerilogTimings", "3.0.1"));
                    project.Packages.Add(new PackageModel("Microsoft.AspNetCore.Authentication.JwtBearer", "6.5.0"));
                    project.Packages.Add(new PackageModel("System.IdentityModel.Tokens.Jwt", "6.25.1"));
                    break;

                case Constants.ProjectType.Domain:
                    project.Packages.Add(new PackageModel("FluentValidation", "11.4.0"));
                    project.Packages.Add(new PackageModel("MediatR", "12.0.0"));
                    project.Packages.Add(new PackageModel("Newtonsoft.Json", "13.0.2"));
                    project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore", "7.0.2"));
                    break;

                case Constants.ProjectType.Application:
                    break;

                case Constants.ProjectType.Infrastructure:

                    var tokens = new TokensBuilder()
                        .With("serviceName", serviceName)
                        .Build();

                    project.DotNetProjectType = DotNetProjectType.ClassLib;
                    project.Folders.Add(new FolderModel("Data", project.Directory));
                    project.Files.Add(_fileModelFactory.CreateTemplate("DddApp.Infrastructure.ConfigureServices", "ConfigureServices", project.Directory, "cs", tokens: tokens));

                    project.Files.Add(_fileModelFactory.CreateTemplate("DddApp.Infrastructure.SeedData", "SeedData", Path.Combine(project.Directory, "Data"), "cs", tokens: new TokensBuilder()
                        .With("serviceName", serviceName)
                        .With("namespace", $"{serviceName}.Infrastructure.Data")
                        .Build()));

                    project.Files.Add(_fileModelFactory.CreateTemplate("DddApp.Infrastructure.DesignTimeDbContextFactory", "DesignTimeDbContextFactory", project.Directory, "cs", tokens: tokens));


                    project.References.Add($"..{Path.DirectorySeparatorChar}{serviceName}.Core{Path.DirectorySeparatorChar}{serviceName}.Core.csproj");

                    project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore.Tools", "7.0.0"));
                    project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore.SqlServer", "7.0.2"));
                    project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore.Design", "7.0.2"));
                    break;

                case Constants.ProjectType.Api:

                    project.DotNetProjectType = DotNetProjectType.WebApi;

                    project.Folders.Add(new("Controllers", project.Directory));

                    project.References.Add($"..{Path.DirectorySeparatorChar}{serviceName}.Infrastructure{Path.DirectorySeparatorChar}{serviceName}.Infrastructure.csproj");

                    project.Files.Add(_fileModelFactory.CreateTemplate("DddApp.Api.AppSettings", "appsettings", project.Directory, "json", tokens: new TokensBuilder().With("serviceName", serviceName).Build()));

                    project.Files.Add(_fileModelFactory.CreateTemplate("Api.ConfigureServices", "ConfigureServices", project.Directory, tokens: new TokensBuilder()
                        .With("DbContext", $"{serviceName}DbContext")
                        .With("serviceName", serviceName)
                        .Build()));

                    project.Files.Add(_fileModelFactory.CreateTemplate("Api.Program", "Program", project.Directory, tokens: new TokensBuilder()
                        .With("DbContext", $"{serviceName}DbContext")
                        .With("serviceName", serviceName)
                        .Build()));

                    project.Packages.Add(new PackageModel("Microsoft.AspNetCore.Mvc.Versioning", "5.0.0"));
                    project.Packages.Add(new PackageModel() { Name = "Microsoft.AspNetCore.OpenApi", Version = "7.0.2" });
                    project.Packages.Add(new PackageModel("Serilog", "2.12.0"));
                    project.Packages.Add(new PackageModel("SerilogTimings", "3.0.1"));
                    project.Packages.Add(new PackageModel("Serilog.AspNetCore", "6.0.1"));
                    project.Packages.Add(new PackageModel("Swashbuckle.AspNetCore", "6.5.0"));
                    project.Packages.Add(new PackageModel("Swashbuckle.AspNetCore.Annotations", "6.5.0"));
                    project.Packages.Add(new PackageModel("Swashbuckle.AspNetCore.Newtonsoft", "6.5.0"));
                    project.Packages.Add(new PackageModel("Swashbuckle.AspNetCore.Swagger", "6.5.0"));
                    break;

            }
        }
        return project;
    }

    public ProjectModel CreateWebApi(string name, string parentDirectory, List<string> additionalMetadata = null)
    {
        var project = new ProjectModel(name, parentDirectory)
        {
            DotNetProjectType = DotNetProjectType.WebApi
        };

        if (additionalMetadata != null)
            project.Metadata.Concat(additionalMetadata);

        return project;
    }

    public ProjectModel CreateTestingProject()
    {
        throw new NotImplementedException();
    }

    public ProjectModel CreateUnitTestsProject()
    {
        throw new NotImplementedException();
    }

    public ProjectModel CreateIntegrationTestsProject()
    {
        throw new NotImplementedException();
    }

    public ProjectModel CreateMessagingProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Messaging", directory);

        model.Files.Add(_fileModelFactory.CreateTemplate("IMessagingClient", "IMessagingClient", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("IServiceBusMessage", "IServiceBusMessage", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("IServiceBusMessageListener", "IServiceBusMessageListener", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("IServiceBusMessageSender", "IServiceBusMessageSender", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("ReceiveRequest", "ReceiveRequest", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("ServiceBusMessage", "ServiceBusMessage", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("Observable", "Observable", $"{model.Directory}{Path.DirectorySeparatorChar}Internals"));

        model.Packages.Add(new PackageModel() { Name = "MediatR.Contracts", Version = "1.0.1" });

        model.Packages.Add(new PackageModel() { Name = "Newtonsoft.Json", Version = "13.0.2" });

        return model;
    }

    public ProjectModel CreateMessagingUdpProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Messaging.Udp", directory);

        model.References.Add(@"..\Messaging\Messaging.csproj");

        model.Packages.Add(new PackageModel() { Name = "Microsoft.Extensions.DependencyInjection.Abstractions", Version = "7.0.0" });

        model.Packages.Add(new PackageModel() { Name = "Microsoft.Extensions.Logging.Abstractions", Version = "7.0.0" });

        model.Packages.Add(new PackageModel() { Name = "System.Reactive.Linq", Version = "5.0.0" });

        model.Files.Add(_fileModelFactory.CreateTemplate("Messaging.Udp.ConfigureServices", "ConfigureServices", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("IUdpClientFactory", "IUdpClientFactory", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("MessagingClient", "MessagingClient", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("ServiceBusMessageListener", "ServiceBusMessageListener", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("ServiceBusMessageSender", "ServiceBusMessageSender", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("UdpClientFactory", "UdpClientFactory", model.Directory));

        return model;
    }

    public ProjectModel CreateValidationProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Validation", directory);

        return model;
    }

    public ProjectModel CreateKernelProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Kernel", directory);

        model.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore", "7.0.2"));

        var responseBase = new ClassModel("ResponseBase");

        var responseBaseConstructor = new ConstructorModel(responseBase, responseBase.Name)
        {
            Body = "Errors = new List<string>();"
        };

        responseBase.Properties.Add(new PropertyModel(responseBase, AccessModifier.Public, TypeModel.ListOf("string"), "Errors", PropertyAccessorModel.GetPrivateSet));

        responseBase.Constructors.Add(responseBaseConstructor);

        var entityFrameworFileModel = _fileModelFactory.CreateTemplate("BuildingBlocks.Kernel.EntityFrameworkCoreExtensions", "EntityFrameworkCoreExtensions", model.Directory);


        model.Files.Add(entityFrameworFileModel);

        model.Files.Add(new ObjectFileModel<ClassModel>(responseBase, responseBase.UsingDirectives, responseBase.Name, model.Directory, "cs"));

        return model;
    }

    public ProjectModel CreateSecurityProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Security", directory);

        model.Packages.Add(new PackageModel { Name = "MediatR", Version = "12.0.0" });

        model.Packages.Add(new PackageModel { Name = "Microsoft.AspNetCore.Authentication.JwtBearer", Version = "7.0.2" });

        model.Packages.Add(new PackageModel { Name = "Swashbuckle.AspNetCore.SwaggerGen", Version = "6.5.0" });

        model.Packages.Add(new PackageModel { Name = "System.IdentityModel.Tokens.Jwt", Version = "6.25.1" });

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.AccessRight", "AccessRight", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.Authentication", "Authentication", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.AuthorizationHeaderParameterOperationFilter", "AuthorizationHeaderParameterOperationFilter", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.AuthorizeResourceOperationAttribute", "AuthorizeResourceOperationAttribute", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.ConfigureServices", "ConfigureServices", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.IPasswordHasher", "IPasswordHasher", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.ITokenBuilder", "ITokenBuilder", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.ITokenProvider", "ITokenProvider", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.Operations", "Operations", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.PasswordHasher", "PasswordHasher", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.ResourceOperationAuthorizationBehavior", "ResourceOperationAuthorizationBehavior", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.ResourceOperationAuthorizationHandler", "ResourceOperationAuthorizationHandler", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.SecurityConstants", "SecurityConstants", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.TokenBuilder", "TokenBuilder", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("BuildingBlocks.Security.Security.TokenProvider", "TokenProvider", model.Directory));

        return model;
    }

    public ProjectModel Create(string type, string name, string directory, List<string> references = null, string metadata = null)
    {
        var model = CreateLibrary(name, directory, metadata?.Split(',').ToList());

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

        if (type.StartsWith("web"))
        {
            model.Files.Add(_fileModelFactory.CreateTemplate("DefaultProgram", "Program", model.Directory, tokens: tokens));

            model.Files.Add(_fileModelFactory.CreateTemplate("DefaultConfigureServices", "ConfigureServices", model.Directory, tokens: tokens));

            model.Packages.Add(new PackageModel() { Name = "Microsoft.AspNetCore.OpenApi", Version = "7.0.2" });

            model.Packages.Add(new PackageModel() { Name = "Swashbuckle.AspNetCore", Version = "6.4.0" });
        }

        return model;
    }

    public ProjectModel CreateCore(string name, string directory)
        => CreateLibrary($"{name}.Core", directory, new() { Constants.ProjectType.Core });

    public ProjectModel CreateInfrastructure(string name, string directory)
        => CreateLibrary($"{name}.Infrastructure", directory, new() { Constants.ProjectType.Infrastructure });

    public ProjectModel CreateApi(string name, string directory)
        => CreateLibrary($"{name}.Api", directory, new() { Constants.ProjectType.Api });

}

