using Endpoint.Core.Abstractions;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit;

namespace Endpoint.UnitTests;

public class ClassSyntaxGenerationStrategyTests
{
    [Fact]
    public void CreateShouldSerializeToExpectedString()
    {
        var expected = "public class Foo { }";

        var services = new ServiceCollection();

        services.AddLogging();

        services.AddCliServices();

        var container = services.BuildServiceProvider();

        var syntaxGenerationStrategyFactory = container.GetRequiredService<ISyntaxGenerationStrategyFactory>();

        var sut = ActivatorUtilities.CreateInstance<ClassSyntaxGenerationStrategy>(container);

        var classModel = new ClassModel("Foo");

        classModel.Constructors = new List<ConstructorModel>()
        {
            new ConstructorModel(classModel,classModel.Name)
        };

        var result = sut.Create(syntaxGenerationStrategyFactory, classModel);

        Assert.Equal(expected, result);

    }

    [Fact]
    public void CreateShouldSerializeToExpectedString_GivenAField()
    {
        var expected = "public class Foo { }";

        var services = new ServiceCollection();

        services.AddLogging();

        services.AddCliServices();

        var container = services.BuildServiceProvider();

        var syntaxGenerationStrategyFactory = container.GetRequiredService<ISyntaxGenerationStrategyFactory>();

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

        var result = sut.Create(syntaxGenerationStrategyFactory, classModel);

        Assert.Equal(expected, result);

    }
}
