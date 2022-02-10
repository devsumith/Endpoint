using System.Collections.Generic;
using Endpoint.SharedKernal;

namespace Endpoint.Core.Builders
{
    public class SwaggerAnnotationBuilder
    {
        private int _indent = 0;
        private string _summary;
        private string _description;
        public SwaggerAnnotationBuilder(string summary, string description, int indent = 0)
        {
            _summary = summary;
            _description = description;
            _indent = indent;
        }

        public string[] Build()
        {
            var results = new List<string>()
            {
                "[SwaggerOperation(".Indent(_indent)
            };

            results.Add($"Summary = \"{_summary}\",".Indent(_indent + 1));

            results.Add($"Description = @\"{_description}\"".Indent(_indent + 1));

            results.Add(")]".Indent(_indent));

            return results.ToArray();
        }
    }
}
