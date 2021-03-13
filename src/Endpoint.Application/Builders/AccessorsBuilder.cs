using System.Text;

namespace Endpoint.Application.Builders
{
    public class AccessorsBuilder
    {
        private StringBuilder _string;
        private string _accessor;

        public AccessorsBuilder()
        {
            _string = new StringBuilder();
            _accessor = "{ get; set; }";
        }
        public string Build()
        {
            return _string
                .Append(_accessor)
                .ToString();
        }
    }
}
