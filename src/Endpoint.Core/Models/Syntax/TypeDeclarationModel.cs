using Endpoint.Core.Models.Syntax.Properties;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax;

public class TypeDeclarationModel
{
	public TypeDeclarationModel(string name)
	{
		Name = name;
		Properties = new List<PropertyModel>();
		UsingDirectives = new List<UsingDirectiveModel>();
	}

    public string Name { get; set; }
    public List<PropertyModel> Properties { get; set; }

    public List<UsingDirectiveModel> UsingDirectives { get; set; }
}
