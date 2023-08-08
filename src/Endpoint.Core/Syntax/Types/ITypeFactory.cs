using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Types;

public interface ITypeFactory
{
    Task<TypeModel> Create(ClassModel @class);
}

