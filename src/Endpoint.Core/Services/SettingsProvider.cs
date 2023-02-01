// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Exceptions;
using Endpoint.Core.Models.Options;
using System.IO;
using System.Linq;
using System.Text.Json;
using static System.Environment;
using static System.IO.Path;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Core.Services;

public class SettingsProvider : ISettingsProvider
{
    public SettingsModel Get(string directory = null)
    {
        directory ??= CurrentDirectory;

        var parts = directory.Split(DirectorySeparatorChar);

        for (var i = 1; i <= parts.Length; i++)
        {
            var path = $"{string.Join(DirectorySeparatorChar, parts.Take(i))}{DirectorySeparatorChar}{Constants.SettingsFileName}";

            if (File.Exists(path))
            {
                var settings = Deserialize<SettingsModel>(File.ReadAllText(path), new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
                settings.RootDirectory = new FileInfo(path).Directory.FullName;
                return settings;
            }
        }

        return null;

    }
}

