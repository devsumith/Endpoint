// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Endpoint.Core.Syntax.Angular;
using Newtonsoft.Json.Linq;

namespace Endpoint.Core.Extensions;

public static class JObjectExtensions
{
    public static void AddBuildConfiguration(this JObject jObject, string configurationName, string projectName, List<FileReplacementModel> fileReplacements)
    {
        var configurationObject = new JObject();

        var fileReplacementArray = new JArray();

        foreach (var entry in fileReplacements)
        {
            var fileReplacementEntry = new JObject();

            fileReplacementEntry["replace"] = entry.Replace;

            fileReplacementEntry["with"] = entry.With;

            fileReplacementArray.Add(fileReplacementEntry);
        }

        configurationObject["fileReplacements"] = fileReplacementArray;

        jObject["projects"][projectName]["architect"]["build"]["configurations"][configurationName] = configurationObject;
    }

    public static void ExportsAssetsAndStyles(this JObject jObject)
    {
        jObject["assets"] = new JArray("./scss/*.*", "./assets/**/*.*");
    }

    public static void EnableDefaultStandalone(this JObject jObject, string projectName)
    {
        jObject["projects"][projectName]["schematics"] = new JObject
        {
            {
                "@schematics/angular:component", new JObject()
            {
                { "standalone", true },
                { "style", "scss" },
            }
            },
            {
                "@schematics/angular:directive", new JObject()
            {
                { "standalone", true },
            }
            },
        };
    }

    public static void UpdateCompilerOptionsToUseJestTypes(this JObject jObject)
    {
        var types = jObject["compilerOptions"]["types"] as JArray;

        types.Clear();

        types.Add("jest");
    }

    public static void AddScript(this JObject jObject, string name, string value)
    {
        var scripts = jObject["scripts"] as JObject;

        scripts.AddOrUpdate(name, value);
    }

    public static void AddScripts(this JObject jObject, IEnumerable<KeyValuePair<string, string>> scripts)
    {
        foreach (var script in scripts)
        {
            jObject.AddScript(script.Key, script.Value);
        }
    }

    public static void AddStyle(this JObject jobject, string projectName, string path)
    {
        var styles = jobject["projects"][projectName]["architect"]["build"]["options"]["styles"] as JArray;

        var o = new JArray { path };

        foreach (var style in styles)
        {
            o.Add(style);
        }

        jobject["projects"][projectName]["architect"]["build"]["options"]["styles"] = o;
    }

    public static void AddAuthor(this JObject jObject, string author)
    {
        jObject["author"] = author;
    }

    public static void RemoveAllScripts(this JObject jObject)
    {
        var scripts = jObject["scripts"] as JObject;

        scripts.RemoveAll();
    }

    public static void AddOrUpdate(this JObject jObject, string key, JToken value)
    {
        if (jObject[key] != null)
        {
            jObject[key] = value;
        }
        else
        {
            jObject.Add(key, value);
        }
    }

    public static void AddSupportedLocales(this JObject jObject, string projectName, List<string> locales = null)
    {
        var localesObject = new JObject();

        var root = $"{jObject["projects"][projectName]["root"]}";

        foreach (var locale in locales)
        {
            localesObject.AddOrUpdate(locale, $"{root}/src/locale/messages.{locale}.xlf");
        }

        var projectJObject = jObject["projects"][projectName] as JObject;

        projectJObject.AddOrUpdate("i18n", new JObject
        {
            { "sourceLocale", "en-US" },
            { "locales", localesObject },
        });

        var buildOptions = jObject["projects"][projectName]["architect"]["build"]["options"] as JObject;

        buildOptions.AddOrUpdate("localize", new JArray(locales));
    }

    public static List<string> GetSupportedLocales(this JObject jObject, string projectName)
    {
        var jArray = jObject["projects"][projectName]["architect"]["build"]["options"]["localize"] as JArray;

        return jArray.Select(x => $"{x}").ToList();
    }

    public static string GetProjectDirectory(this JObject jObject, string projectName)
    {
        return $"{jObject["projects"][projectName]["root"]}";
    }
}
