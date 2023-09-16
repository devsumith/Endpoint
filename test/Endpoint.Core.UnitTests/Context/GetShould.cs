// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.UnitTests.Context;

using Context = Endpoint.Core.Services.Context;

public class GetShould
{
    [Fact]
    public void ReturnExpectedItem()
    {
        var services = new ServiceCollection();

        services.AddLogging();

        var serviceProvider = services.BuildServiceProvider();

        var sut = ActivatorUtilities.CreateInstance<Context>(serviceProvider);

        sut.Set(new Foo() { Name = "Foo" });

        var result = sut.Get<Foo>();

        Assert.Equal("Foo", result.Name);
    }
}

public class Foo
{
    public string Name { get; set; }
}
