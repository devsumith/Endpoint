// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Classes.Strategies;
using Endpoint.Core.Syntax.Constructors;
using Endpoint.Core.Syntax.Fields;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;

namespace Endpoint.UnitTests;

public class ClassSyntaxGenerationStrategyTests
{
    [Fact]
    public async Task CreateShouldSerializeToExpectedString()
    {
        var expected = "public class Foo { }";

        var services = new ServiceCollection();

        services.AddLogging();

        services.AddCliServices();

        var container = services.BuildServiceProvider();

        var syntaxGenerator = container.GetRequiredService<ISyntaxGenerator>();

        var sut = ActivatorUtilities.CreateInstance<ClassSyntaxGenerationStrategy>(container);

        var classModel = new ClassModel("Foo");

        classModel.Constructors = new List<ConstructorModel>()
        {
            new ConstructorModel(classModel,classModel.Name)
        };

        var result = await sut.GenerateAsync(syntaxGenerator, classModel);

        Assert.Equal(expected, result);

    }

    [Fact]
    public async Task CreateConfigureServicesClass()
    {
        var expected = "public class Foo { }";

        var services = new ServiceCollection();

        services.AddLogging();

        services.AddCliServices();

        var container = services.BuildServiceProvider();

        var syntaxGenerator = container.GetRequiredService<ISyntaxGenerator>();

        var sut = ActivatorUtilities.CreateInstance<ClassSyntaxGenerationStrategy>(container);

        var classModel = new ClassModel("ConfigureServices");

        var methodParam = new ParamModel()
        {
            Type = new TypeModel("IServiceCollection"),
            Name = "services",
            ExtensionMethodParam = true
        };

        var method = new MethodModel()
        {
            Name = "AddApplicationServices",
            ReturnType = new TypeModel("void"),
            Static = true,
            Params = new List<ParamModel>() { methodParam }
        };

        classModel.Static = true;

        classModel.Methods.Add(method);

        var result = await sut.GenerateAsync(syntaxGenerator, classModel);

        Assert.Equal(expected, result);

    }

    [Fact]
    public async Task CreateShouldSerializeToExpectedString_GivenAField()
    {
        var expected = "public class Foo { }";

        var services = new ServiceCollection();

        services.AddLogging();

        services.AddCliServices();

        var container = services.BuildServiceProvider();

        var syntaxGenerator = container.GetRequiredService<ISyntaxGenerator>();

        var sut = ActivatorUtilities.CreateInstance<ClassSyntaxGenerationStrategy>(container);

        var classModel = new ClassModel("Foo");

        classModel.Fields = new List<FieldModel>()
        {
            new FieldModel() { Name = "_logger", Type = new TypeModel() {
                Name = "ILogger",
                GenericTypeParameters = new List<TypeModel>
                {
                    new TypeModel() { Name = classModel.Name }
                }

            }, AccessModifier = AccessModifier.Private }
        };

        classModel.Constructors = new List<ConstructorModel>()
        {
            new ConstructorModel(classModel,classModel.Name)
            {
                Params = new List<ParamModel>()
                {
                    new ParamModel
                    {
                        Type = new TypeModel() {
                            Name = "ILogger",
                            GenericTypeParameters = new List<TypeModel>
                            {
                                new TypeModel() { Name = classModel.Name }
                            }
                        },
                        Name = "logger"
                    }
                }
            }
        };

        var result = await sut.GenerateAsync(syntaxGenerator, classModel);

        Assert.Equal(expected, result);

    }
}

