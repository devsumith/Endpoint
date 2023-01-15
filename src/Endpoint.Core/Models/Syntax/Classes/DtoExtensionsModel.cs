using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Classes;

public class DtoExtensionsModel: ClassModel {
    public DtoExtensionsModel(INamingConventionConverter namingConventionConverter, string name, ClassModel entity)
        :base(name)
    {
        Static = true;

        var methodModel = new MethodModel();

        methodModel.Static = true;

        methodModel.Params.Add(new Params.ParamModel
        {
            ExtensionMethodParam = true,
            Name = namingConventionConverter.Convert(NamingConvention.CamelCase,name),
            Type = new TypeModel(name)
        });

        var builder = new StringBuilder();

        builder.AppendLine($"return new {name}Dto".Indent(1));

        builder.AppendLine("{".Indent(1));

        foreach(var prop in entity.Properties)
        {
            builder.AppendLine($"{name}Id = {namingConventionConverter.Convert(NamingConvention.CamelCase, name)}.{prop.Name}".Indent(2));
        }

        builder.AppendLine("}".Indent(1));

        methodModel.Body = builder.ToString();  

        Methods.Add(methodModel);
    }

}
