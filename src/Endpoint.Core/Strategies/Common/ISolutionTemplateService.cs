using System.Collections.Generic;

namespace Endpoint.Core.Services
{
    public interface ISolutionTemplateService
    {
        void Build(string name, string dbContextName, bool shortIdPropertyName, string resource, string properties, bool isMonolith, bool numericIdPropertyDataType, string directory, List<string> plugins, string prefix);
    }
}
