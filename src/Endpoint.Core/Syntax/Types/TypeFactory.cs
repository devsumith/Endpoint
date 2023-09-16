using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Types;

public class TypeFactory : ITypeFactory
{
    private readonly ILogger<TypeFactory> logger;

    public TypeFactory(ILogger<TypeFactory> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TypeModel> Create(ClassModel @class)
    {
        logger.LogInformation("Create Type");

        return new TypeModel(@class.Name)
        {
            Class = @class,
        };
    }
}
