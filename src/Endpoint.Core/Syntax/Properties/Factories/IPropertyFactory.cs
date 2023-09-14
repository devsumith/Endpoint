using Endpoint.Core.Syntax.Cqrs;
using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Properties.Factories;

public interface IPropertyFactory
{

    Task<List<PropertyModel>> ResponsePropertiesCreateAsync(RequestType responseType, TypeDeclarationModel parent, string entityName);
}

