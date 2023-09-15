// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Endpoint.UnitTests.MethodPlantUmlParsingStrategy;

using MethodPlantUmlParsingStrategy = Endpoint.Core.Syntax.Methods.Strategies.MethodPlantUmlParsingStrategy;

public class ParseAsyncShould
{
    [Fact]
    public async Task ReturnMethodModel_GivenPlantUmlString()
    {
        // ARRANGE
        var services = new ServiceCollection();

        services.AddLogging();

        services.AddSingleton<IContext, Context>();

        var serviceProvider = services.BuildServiceProvider();

        var context = serviceProvider.GetService<IContext>();

        context.Set(new MethodModel() { IsInterface = true });

        var sut = ActivatorUtilities.CreateInstance<MethodPlantUmlParsingStrategy>(serviceProvider);

        // ACT
        var result = await sut.ParseAsync(default, "+void Lock()");

        // ASSERT
        Assert.NotNull(result);
    }

}

