using System.Collections.Generic;

namespace Endpoint.Core.Services
{
    public interface IContext : IDictionary<string, string[]>
    {

    }

    public class Context : Dictionary<string, string[]>, IContext
    {
    }
}
