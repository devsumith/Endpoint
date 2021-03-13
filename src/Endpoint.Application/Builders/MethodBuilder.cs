using System.Collections.Generic;

namespace Endpoint.Application.Builders
{
    public class MethodBuilder
    {
        private List<string> _contents;
        private int _indent;
        private int _tab = 4;
        private string _accessModifier = "public";
        private List<string> _attributes;
        private List<ParemeterInfo> _parameters = new List<ParemeterInfo>();
        public List<string> _body = new List<string>();


        public MethodBuilder()
        {
            _contents = new List<string>();
            _attributes = new List<string>();
            _body = new List<string>();
        }

        public string[] Build()
        {
            return _contents.ToArray();
        }
    }

    public class ParemeterInfo
    {

    }
}
