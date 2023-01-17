using System.Threading.Tasks;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public interface IAggregateService
{
    Task Add(string name, string properties, string directory, string microserviceName);

}

