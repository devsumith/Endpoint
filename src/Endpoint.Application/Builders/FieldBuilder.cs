using System.Collections.Generic;
using System.Text;

namespace Endpoint.Application.Builders
{
    public class FieldBuilder
    {
        private StringBuilder _string;
        private int _indent;
        private int _tab = 4;
        private string _accessModifier = "public";

        public string Build()
        {
            return _string.ToString();
        }
    }
}
