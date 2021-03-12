using Endpoint.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Endpoint.Application.Services
{

    public static class StringListExtensions
    {
        public static string GetResourceName(this string[] collection, string name)
            => collection.SingleOrDefault(x => x.EndsWith(name)) == null ?
                collection.SingleOrDefault(x => x.EndsWith($".{name}.txt"))
                : collection.SingleOrDefault(x => x.EndsWith(name));
    }

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
}