using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Types;

public class TypeFactory: ITypeFactory
{
    private readonly ILogger<TypeFactory> _logger;

    public TypeFactory(ILogger<TypeFactory> logger){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TypeModel> Create(ClassModel @class)
    {
        _logger.LogInformation("Create Type");

        return new TypeModel(@class.Name) {
            Class = @class,
        };       
    }

}

