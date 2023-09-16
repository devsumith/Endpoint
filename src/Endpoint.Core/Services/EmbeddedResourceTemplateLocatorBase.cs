// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Services;

public class EmbeddedResourceTemplateLocatorBase<T> : ITemplateLocator
    where T : class
{
    private readonly ILogger<EmbeddedResourceTemplateLocatorBase<T>> logger;

    public EmbeddedResourceTemplateLocatorBase(ILogger<EmbeddedResourceTemplateLocatorBase<T>> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public string Get(string name)
    {
        logger.LogInformation("Attempting to get template for: {naem}.", name);

        var @namespace = $"{typeof(T).Namespace}";

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().FullName.Contains(@namespace)).Distinct())
        {
            var resourceName = assembly.GetManifestResourceNames().GetResourceName(name);

            if (!string.IsNullOrEmpty(resourceName))
            {
                return GetResource(assembly, resourceName);
            }
        }

        throw new Exception(string.Empty);
    }

    public string GetResource(Assembly assembly, string name)
    {
        logger.LogInformation("Attempting to get resource for: {naem}.", name);

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

            return string.Join(Environment.NewLine, lines);
        }
    }
}
