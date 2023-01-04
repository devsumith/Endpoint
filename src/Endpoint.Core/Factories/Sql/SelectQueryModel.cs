using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Factories.Sql
{
    public static class SelectQueryModelFactory
    {
        public static SelectQueryModel Create()
        {
            var model = new SelectQueryModel(); 

            return model;
        }
    }
}
