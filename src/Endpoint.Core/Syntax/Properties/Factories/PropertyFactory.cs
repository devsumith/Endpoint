using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Cqrs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using static Endpoint.Core.Syntax.AccessModifier;
using static Endpoint.Core.Syntax.Properties.PropertyAccessorModel;
using static Endpoint.Core.Syntax.Types.TypeModel;

namespace Endpoint.Core.Syntax.Properties.Factories;

public class PropertyFactory : IPropertyFactory
{
    private readonly ILogger<PropertyFactory> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;

    public PropertyFactory(ILogger<PropertyFactory> logger, INamingConventionConverter namingConventionConverter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public async Task<List<PropertyModel>> ResponsePropertiesCreateAsync(RequestType responseType, TypeDeclarationModel parent, string entityName)
    {
        _logger.LogInformation("Creating Response Properties. {entityName}", entityName);

        var model = new List<PropertyModel>();

        switch (responseType)
        {
            case RequestType.Get:
                var entityNamePascalCasePlural = _namingConventionConverter.Convert(NamingConvention.PascalCase, entityName, pluralize: true);

                model.Add(new(parent, Public, ListOf($"{entityName}Dto"), entityNamePascalCasePlural, GetSet));
                break;

            default:
                throw new InvalidOperationException();
        }

        return model;
    }

    /*    public async Task<List<PropertyModel>> ResponsePropertiesCreateAsync(TypeDeclarationModel parent, string properties)
        {
            _logger.LogInformation("Creating Properties");

            var model = new List<PropertyModel>();

            var props = properties.Split(',');

            foreach(var prop in props) { 

                var parts = prop.Split(':');

                model.Add(new (parent, AccessModifier.Public, new(parts[1]), parts[0], PropertyAccessorModel.GetSet));
            }

            return model;
        }*/

}

