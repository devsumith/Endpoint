using System.Collections.Generic;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Interfaces;
using Endpoint.Core.Syntax.Types;

namespace Endpoint.Core.Syntax.Properties;

public class PropertyModel : SyntaxModel
{
    public PropertyModel(TypeDeclarationModel parent, AccessModifier accessModifier, TypeModel type, string name, List<PropertyAccessorModel> accessors, bool required = false, bool key = false)
    {
        AccessModifier = accessModifier;
        Type = type;
        Accessors = accessors;
        Name = name;
        Required = required;
        Id = key;
        Parent = parent;
        Interface = parent is InterfaceModel;
    }

    public AccessModifier AccessModifier { get; private set; }

    public TypeModel Type { get; private set; }

    public List<PropertyAccessorModel> Accessors { get; private set; } = new ();

    public string Name { get; private set; }

    public bool Required { get; private set; }

    public bool Id { get; private set; }

    public bool Interface { get; set; }

    public TypeDeclarationModel Parent { get; set; }

    public string DefaultValue { get; set; }

    public bool IsClassProperty => Parent is ClassModel;

    public static PropertyModel TypeScriptProperty(string name, string type)
    {
        return new PropertyModel(null, default, new TypeModel(type), name, null);
    }

    public PropertyModel ToTs()
    {
        var model = TypeScriptProperty(Name, Type.Name);

        switch (model.Type.Name.ToLower())
        {
            case "guid":
                model.Type.Name = "string";
                break;

            case "int":
                model.Type.Name = "number";
                break;
        }

        return model;
    }

    public PropertyModel(TypeModel type, string name, PropertyAccessorModel accessor)
    {
        Type = type;
        Accessors = new List<PropertyAccessorModel> { accessor };
        Name = name;
        Interface = true;
    }
}
