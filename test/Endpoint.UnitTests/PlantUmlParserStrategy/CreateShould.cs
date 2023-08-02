// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Fields;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Properties;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Endpoint.UnitTests.PlantUmlParserStrategy;
public class CreateShould
{


    [Fact]
    public void CreateSolution_GivenAPackageInPlantUml()
    {
        var services = new ServiceCollection();

        services.AddCoreServices();

        var container = services.BuildServiceProvider();

        var sut = container.GetRequiredService<IPlantUmlParserStrategyFactory>();

        var result = sut.CreateFor(@"@startuml

package DashboardService  {
'dotNetType:classlib

class Order {

}

}

'classlib
package DashboardService  {

class Order {

}

}
");

        Assert.Equal(typeof(SolutionModel), result.GetType());

    }

    [Fact]
    public void CreateClass_GivenAPackageInPlantUml()
    {
        var services = new ServiceCollection();

        services.AddCoreServices();

        var container = services.BuildServiceProvider();

        var sut = container.GetRequiredService<IPlantUmlParserStrategyFactory>();

        var result = sut.CreateFor(@"class User {
+Task ExecuteAsync(CancellationToken stoppingToken)
-string Username
-string Password
}");

        Assert.Equal(typeof(ObjectFileModel<ClassModel>), result.GetType());

    }

    [Fact]
    public void CreateMethod_GivenAPackageInPlantUml()
    {
        var services = new ServiceCollection();

        services.AddCoreServices();

        var container = services.BuildServiceProvider();

        var sut = container.GetRequiredService<IPlantUmlParserStrategyFactory>();

        var result = sut.CreateFor(@"+Task ExecuteAsync(CancellationToken stoppingToken)");

        Assert.Equal(typeof(MethodModel), result.GetType());
    }

    [Fact]
    public void CreateProperty_GivenAPackageInPlantUml()
    {
        var services = new ServiceCollection();

        services.AddCoreServices();

        var container = services.BuildServiceProvider();

        var sut = container.GetRequiredService<IPlantUmlParserStrategyFactory>();

        var result = sut.CreateFor(@"-int Id");

        Assert.Equal(typeof(PropertyModel), result.GetType());
    }

    [Fact]
    public void CreateField_GivenAPackageInPlantUml()
    {
        var services = new ServiceCollection();

        services.AddCoreServices();

        var container = services.BuildServiceProvider();

        var sut = container.GetRequiredService<IPlantUmlParserStrategyFactory>();

        var result = sut.CreateFor(@"-Task ExecuteAsync(CancellationToken stoppingToken)");

        Assert.Equal(typeof(FieldModel), result.GetType());
    }
}

