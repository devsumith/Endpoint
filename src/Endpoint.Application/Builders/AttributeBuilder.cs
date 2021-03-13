using System.Text;

namespace Endpoint.Application.Builders
{
    public class AttributeBuilder
    {
        private StringBuilder _string;
        private int _indent;
        private int _tab = 4;

        public string Build()
        {
            return _string.ToString();
        }
    }
}
