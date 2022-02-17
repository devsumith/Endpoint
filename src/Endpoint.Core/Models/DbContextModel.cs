using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public  class DbContextModel
    {
        public string Name { get; set; }
        public List<AggregateRootModel> AggregateRoots { get; private set; } = new List<AggregateRootModel>();

        public DbContextModel(string name, List<AggregateRootModel> aggregateRoots)
        {
            Name = name;
            AggregateRoots = aggregateRoots;
        }
    }
}
