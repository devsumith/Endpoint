using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;

namespace Endpoint.Application.Builders.CSharp
{
    public class ClassBuilder
    {
        private readonly IContext _context;
        public ClassBuilder(IContext context)
        {
            _context = context;
        }


        public void Build()
        {

        }
    }
}
