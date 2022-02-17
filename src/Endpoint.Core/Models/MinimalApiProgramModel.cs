using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class MinimalApiProgramModel
    {
        public string ApiNamespace { get; set; }
        public List<string> Usings { get; private set; } = new List<string>()
        {
            "Microsoft.EntityFrameworkCore",
            "Microsoft.OpenApi.Models",
            "System.Reflection"
        };

        public  string DbContextName { get; private set; }

        public List<AggregateRoot> AggregateRoots { get; private set; } = new List<AggregateRoot>();

        public MinimalApiProgramModel(string apiNamespace, List<AggregateRoot> aggregateRoots)
        {
            AggregateRoots = aggregateRoots;
            ApiNamespace = apiNamespace;
        }
    }
}
