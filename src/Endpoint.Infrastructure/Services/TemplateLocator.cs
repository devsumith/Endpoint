using Endpoint.Core;
using Endpoint.Core.Services;
using System.Reflection;

namespace Endpoint.Infrastructure.Services;

public class TemplateLocator : ITemplateLocator
{
    public TemplateLocator()
    {

    }
    public string[] Get(string name)
    {
        foreach (Assembly _assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().FullName.Contains(Constants.RootNamespace)).Distinct())
        {
            var resourceName = _assembly.GetManifestResourceNames().GetResourceName(name);

            if (!string.IsNullOrEmpty(resourceName))
            {
                return GetResource(_assembly, resourceName);
            }
        }

        throw new Exception("");
    }

    public string[] GetResource(Assembly assembly, string name)
    {
        var lines = new List<string>();

        using (var stream = assembly.GetManifestResourceStream(name))
        {
            using (var streamReader = new StreamReader(stream))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines.ToArray();
        }
    }
}