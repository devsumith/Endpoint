// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Enums;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Artifacts.Projects.Enums;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Options;
using Octokit;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Projects.Factories;

public class ProjectModelFactory : IProjectModelFactory
{
    private readonly IFileModelFactory _fileModelFactory;

    public ProjectModelFactory(IFileModelFactory fileModelFactory)
    {
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
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

        if (additionalMetadata != null)
        {
            foreach(var m in additionalMetadata)
            {
                project.Metadata.Add(m);
            }
        }
        

        foreach (var metadataItem in project.Metadata)
        {
            switch (metadataItem)
            {
                case Constants.ProjectType.Core:
                    project.Packages.Add(new PackageModel("FluentValidation", "11.4.0"));
                    project.Packages.Add(new PackageModel("MediatR", "11.0.0"));
                    project.Packages.Add(new PackageModel("Newtonsoft.Json", "13.0.2"));
                    project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore", "7.0.2"));
                    project.Packages.Add(new PackageModel("MediatR.Extensions.Microsoft.DependencyInjection", "11.0.0"));
                    project.Packages.Add(new PackageModel("Microsoft.Extensions.Hosting.Abstractions", "7.0.0"));
                    break;

                case Constants.ProjectType.Domain:
                    project.Packages.Add(new PackageModel("FluentValidation", "11.4.0"));
                    project.Packages.Add(new PackageModel("MediatR", "11.0.0"));
                    project.Packages.Add(new PackageModel("Newtonsoft.Json", "13.0.2"));
                    project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore", "7.0.2"));
                    break;

                case Constants.ProjectType.Application:
                    project.Packages.Add(new PackageModel("MediatR.Extensions.Microsoft.DependencyInjection", "11.0.0"));
                    break;

                case Constants.ProjectType.Infrastructure:
                    project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore.Tools", "7.0.0"));
                    project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore.SqlServer", "7.0.2"));
                    break;

                case Constants.ProjectType.Api:
                    project.Packages.Add(new PackageModel("MediatR.Extensions.Microsoft.DependencyInjection", "11.0.0"));
                    project.Packages.Add(new PackageModel("Serilog", "2.12.0"));
                    project.Packages.Add(new PackageModel("SerilogTimings", "3.0.1"));
                    project.Packages.Add(new PackageModel("Serilog.AspNetCore", "6.0.1"));
                    project.Packages.Add(new PackageModel("Swashbuckle.AspNetCore", "6.4.0"));
                    project.Packages.Add(new PackageModel("Swashbuckle.AspNetCore.Annotations", "6.4.0"));
                    project.Packages.Add(new PackageModel("Swashbuckle.AspNetCore.Newtonsoft", "6.4.0"));
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

        var responseBase = new ClassModel("ResponseBase");

        var responseBaseConstructor = new ConstructorModel(responseBase, responseBase.Name)
        {
            Body = "Errors = new List<string>();"
        };

        responseBase.Properties.Add(new PropertyModel(responseBase, AccessModifier.Public, TypeModel.ListOf("string"), "Errors", PropertyAccessorModel.GetPrivateSet));

        responseBase.Constructors.Add(responseBaseConstructor);

        model.Files.Add(new ObjectFileModel<ClassModel>(responseBase, responseBase.UsingDirectives, responseBase.Name, model.Directory, "cs"));

        return model;
    }

    public ProjectModel CreateSecurityProject(string directory)
    {
        var model = new ProjectModel(DotNetProjectType.ClassLib, "Security", directory);

        model.Packages.Add(new PackageModel { Name = "MediatR", Version = "11.1.0" });

        model.Packages.Add(new PackageModel { Name = "Microsoft.AspNetCore.Authentication.JwtBearer", Version = "7.0.2" });

        model.Packages.Add(new PackageModel { Name = "Swashbuckle.AspNetCore.SwaggerGen", Version = "6.5.0" });

        model.Packages.Add(new PackageModel { Name = "System.IdentityModel.Tokens.Jwt", Version = "6.25.1" });

        model.Files.Add(_fileModelFactory.CreateTemplate("AccessRight", "AccessRight", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("Authentication", "Authentication", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("AuthorizationHeaderParameterOperationFilter", "AuthorizationHeaderParameterOperationFilter", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("AuthorizeResourceOperationAttribute", "AuthorizeResourceOperationAttribute", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("ConfigureServices", "ConfigureServices", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("IPasswordHasher", "IPasswordHasher", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("ITokenBuilder", "ITokenBuilder", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("ITokenProvider", "ITokenProvider", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("Operations", "Operations", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("PasswordHasher", "PasswordHasher", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("ResourceOperationAuthorizationBehavior", "ResourceOperationAuthorizationBehavior", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("ResourceOperationAuthorizationHandler", "ResourceOperationAuthorizationHandler", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("SecurityConstants", "SecurityConstants", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("TokenBuilder", "TokenBuilder", model.Directory));

        model.Files.Add(_fileModelFactory.CreateTemplate("TokenProvider", "TokenProvider", model.Directory));

        return model;
    }
}

