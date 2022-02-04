using System.Collections.Generic;

namespace Endpoint.SharedKernal.Services
{
    public interface IContext : IDictionary<string, string[]>
    {

    }

    public class Context : Dictionary<string, string[]>, IContext
    {
    }
}
