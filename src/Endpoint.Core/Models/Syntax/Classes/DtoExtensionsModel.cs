// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.Text;


namespace Endpoint.Core.Models.Syntax.Classes;

public class DtoExtensionsModel : ClassModel
{
    public DtoExtensionsModel(INamingConventionConverter namingConventionConverter, string name, ClassModel entity)
        : base(name)
    {
        Static = true;

        var toDtoMethodBodyBuilder = new StringBuilder();

        toDtoMethodBodyBuilder.AppendLine($"return new {entity.Name}Dto");

        toDtoMethodBodyBuilder.AppendLine("{");

        foreach (var prop in entity.Properties)
        {
            toDtoMethodBodyBuilder.AppendLine($"{prop.Name} = {namingConventionConverter.Convert(NamingConvention.CamelCase, entity.Name)}.{prop.Name},".Indent(1));
        }

        toDtoMethodBodyBuilder.AppendLine("};");


        Methods.Add(new MethodModel()
        {
            Name = "ToDto",
            Static = true,
            ReturnType = new TypeModel($"{entity.Name}Dto"),
            Params = new List<ParamModel>
            {
                new()
                {
                    ExtensionMethodParam = true,
                    Name = namingConventionConverter.Convert(NamingConvention.CamelCase,entity.Name),
                    Type = new TypeModel(entity.Name)
                }
            },
            Body = toDtoMethodBodyBuilder.ToString()
        });

        Methods.Add(new MethodModel()
        {
            Name = "ToDtosAsync",
            Async = true,
            Static = true,
            ReturnType = new("Task")
            {
                GenericTypeParameters = new()
                {
                    new ("List")
                    {
                        GenericTypeParameters = new()
                        {
                            new ($"{entity.Name}Dto")
                        }
                    }
                }
            },
            Params = new List<ParamModel>
            {
                new()
                {
                    ExtensionMethodParam = true,
                    Name = namingConventionConverter.Convert(NamingConvention.CamelCase,entity.Name, pluralize: true),
                    Type = new TypeModel("IQueryable")
                    {
                        GenericTypeParameters = new List<TypeModel>
                        {
                            new TypeModel(entity.Name)
                        }
                    }
                },
                new()
                {
                    Name = "cancellationToken",
                    Type = new TypeModel("CancellationToken"),
                    DefaultValue = "null"
                }
            },
            Body = $"return await {namingConventionConverter.Convert(NamingConvention.CamelCase, entity.Name, pluralize: true)}.Select(x => x.ToDto()).ToListAsync(cancellationToken);"
        });


    }

}

