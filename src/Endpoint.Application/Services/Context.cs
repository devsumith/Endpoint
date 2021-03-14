using System.Collections.Generic;

namespace Endpoint.Application.Services
{
    public interface IContext: IDictionary<string,string[]>
    {

    }

    public class Context: Dictionary<string,string[]>, IContext
    {
    }
}
