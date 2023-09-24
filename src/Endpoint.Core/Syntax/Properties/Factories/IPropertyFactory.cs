using System.Collections.Generic;
using Endpoint.Core.Syntax.Documents;

namespace Endpoint.Core.Syntax.Properties.Factories;

public interface IPropertyFactory
{
    Task<List<PropertyModel>> ResponsePropertiesCreateAsync(RequestType responseType, TypeDeclarationModel parent, string entityName);
}
