// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Types;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Properties;
using System.Collections.Generic;
using Xunit;
using System.Text;
using System;

namespace Endpoint.UnitTests;


public class TemplateProcessorTests
{
    internal record MyModel(string Title);

    [Fact]
    public void Process_ShouldRenderObject()
    {
        var template = "{{ Title }}";

        var sut = new LiquidTemplateProcessor();

        var result = sut.Process(template, new MyModel("Foo"));

        Assert.Contains("Foo", result);
    }

    [Fact]
    public void Process_ShouldComplextModel()
    {
        var entity = new EntityModel("User");

        entity.Properties = new List<PropertyModel>
        {
            new PropertyModel(entity, AccessModifier.Public,new TypeModel() { Name = "string" },"Name", PropertyAccessorModel.GetPrivateSet)
        };


        var template = new StringBuilder().AppendJoin(Environment.NewLine, new string[9] {
            "{% for using in Usings %}using {{ using }};",
            "{% endfor %}",
            "namespace {{ Namespace }}",
            "{",
            "    public class {{ namePascalCase }}",
            "    {",
            "{% for prop in Properties %}        {{ prop }}{% endfor %}",
            "    }",
            "}"
        }).ToString();

        var sut = new LiquidTemplateProcessor();

        var result = sut.Process(template, new { });

        System.IO.File.WriteAllText(@"C:\reference-architecture\foo.txt", result);

        Assert.Contains("User", result);
    }
}


