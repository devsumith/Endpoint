using Endpoint.Application.Services;
using System.Collections.Generic;

namespace Endpoint.Application.Builders
{
    public class RuleForBuilder
    {

        private readonly Context _context;
        
        public string[] Build()
        {
            var content = new List<string>();

            return content.ToArray();
        }
    }
}
