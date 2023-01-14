using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Classes;

public class DbContextModel : ClassModel
{
    public List<EntityModel> Entities { get; private set; } = new List<EntityModel>();

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

        UsingDirectives.AddRange(new List<UsingDirectiveModel>()
        {
            new UsingDirectiveModel() { Name = $"{serviceName}.Core" },
            new UsingDirectiveModel() { Name = "Microsoft.EntityFrameworkCore" }
        });

        Implements.Add(new TypeModel("DbContext"));

        var ctor = new ConstructorModel(this, Name);

        ctor.Params.Add(new ParamModel()
        {
            Name = "options",
            Type = new TypeModel("DbContextOptions")
            {
                GenericTypeParameters = new List<TypeModel>
                    {
                        new TypeModel(Name)
                    }
            }
        });

        ctor.BaseParams.Add("options");

        Constructors.Add(ctor);

        foreach (var entity in entities)
        {
            Properties.Add(new PropertyModel(
                this,
                Enums.AccessModifier.Public,
                new TypeModel("DbSet")
                {
                    GenericTypeParameters = new List<TypeModel>
                    {
                        new TypeModel(entity.Name)
                    }
                },
                namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, pluralize: true),
                PropertyAccessorModel.GetPrivateSet));



            UsingDirectives.Add(new UsingDirectiveModel { Name = $"{serviceName}.Core.AggregateModel.{entity.Name}Aggregate" });
        }
    }

    public InterfaceModel ToInterface()
    {
        var interfaceModel = this as InterfaceModel;

        interfaceModel.Name = $"I{Name}";

        var saveChangesAsyncMethodModel = new MethodModel();

        saveChangesAsyncMethodModel.Interface = true;

        saveChangesAsyncMethodModel.Params.Add(new ParamModel()
        {
            Type = new TypeModel("CancellationToken"),
            DefaultValue = "default",
            Name = "cancellationToken"
        });

        saveChangesAsyncMethodModel.Name = "SaveChangesAsync";

        saveChangesAsyncMethodModel.ReturnType = new TypeModel("Task")
        {
            GenericTypeParameters = new List<TypeModel>()
            {
                new TypeModel("int")
            }
        };

        interfaceModel.Methods.Add(saveChangesAsyncMethodModel);
        
        return interfaceModel;
    }
}
