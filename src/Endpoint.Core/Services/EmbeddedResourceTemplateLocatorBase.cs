// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Endpoint.Core.Services;

public class EmbeddedResourceTemplateLocatorBase<T> : ITemplateLocator
    where T : class
{
    public string[] Get(string name)
    {
        var @namespace = $"{typeof(T).Namespace}";

        foreach (Assembly _assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().FullName.Contains(@namespace)).Distinct())
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
