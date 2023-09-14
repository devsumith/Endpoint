// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Constructors;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Interfaces;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.Types;
using System.Collections.Generic;
using System.Text;

namespace Endpoint.Core.Syntax.Classes;

public class DbContextModel : ClassModel
{
    public List<EntityModel> Entities { get; private set; } = new List<EntityModel>();
    public string Schema { get; private set; }

    public DbContextModel(
        INamingConventionConverter namingConventionConverter,
        string name,
        List<EntityModel> entities,
        string serviceName
        )
        : base(name)
    {
        Name = name;
        Entities = entities;
        Schema = serviceName.Remove("Service");

        UsingDirectives.AddRange(new UsingModel[]
        {
            new ($"{serviceName}.Core"),
            new ("Microsoft.EntityFrameworkCore")
        });

        Implements.Add(new("DbContext"));

        Implements.Add(new($"I{name}"));

        var ctor = new ConstructorModel(this, Name);

        ctor.Params.Add(new()
        {
            Name = "options",
            Type = new("DbContextOptions")
            {
                GenericTypeParameters = new()
                    {
                        new (Name)
                    }
            }
        });

        ctor.BaseParams.Add("options");

        Constructors.Add(ctor);

        foreach (var entity in entities)
        {
            Properties.Add(new(
                this,
                AccessModifier.Public,
                TypeModel.DbSetOf(entity.Name),
                namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, pluralize: true),
                PropertyAccessorModel.GetPrivateSet));

            UsingDirectives.Add(new($"{serviceName}.Core.AggregatesModel.{entity.Name}Aggregate"));
        }

        var onModelCreatingMethodBodyBuilder = new StringBuilder();
        onModelCreatingMethodBodyBuilder.AppendLine($"modelBuilder.HasDefaultSchema(\"{Schema}\");");
        onModelCreatingMethodBodyBuilder.AppendLine("");
        onModelCreatingMethodBodyBuilder.AppendLine("base.OnModelCreating(modelBuilder);");

        MethodModel onModelCreatingMethod = new()
        {
            AccessModifier = AccessModifier.Protected,
            Name = "OnModelCreating",
            Override = true,
            Params = new List<ParamModel> {
                new () { Type = new ("ModelBuilder"), Name = "modelBuilder" }
            },
            Body = new Syntax.Expressions.ExpressionModel(onModelCreatingMethodBodyBuilder.ToString())
        };

        Methods.Add(onModelCreatingMethod);
    }

    public InterfaceModel ToInterface()
    {
        InterfaceModel interfaceModel = new($"I{Name}");

        interfaceModel.UsingDirectives = UsingDirectives;

        foreach (var prop in Properties)
        {
            interfaceModel.Properties.Add(new(interfaceModel, prop.AccessModifier, prop.Type, prop.Name, prop.Accessors));
        }

        var saveChangesAsyncMethodModel = new MethodModel();

        saveChangesAsyncMethodModel.Interface = true;

        saveChangesAsyncMethodModel.Params.Add(ParamModel.CancellationToken);

        saveChangesAsyncMethodModel.Name = "SaveChangesAsync";

        saveChangesAsyncMethodModel.ReturnType = TypeModel.TaskOf("int");

        interfaceModel.Methods.Add(saveChangesAsyncMethodModel);

        return interfaceModel;
    }
}

