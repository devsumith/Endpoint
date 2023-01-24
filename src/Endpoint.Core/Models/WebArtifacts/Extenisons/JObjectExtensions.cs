using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Linq;

public static class JObjectExtensions
{
    public static void EnableDefaultStandaloneComponents(this JObject jObject, string projectName)
    {
        jObject["projects"][projectName]["schematics"] = new JObject
        {
            { "@schematics/angular:component", new JObject() {
                { "standalone", true },
                { "style", "scss" },
                { "strict", false }
            }
            }
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

        scripts.Add(name, value);
    }

    public static void AddScripts(this JObject jObject, IEnumerable<KeyValuePair<string,string>> scripts)
    {
        foreach(var script in scripts)
        {
            jObject.AddScript(script.Key, script.Value);
        }
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


    public static void AddSupportedLocales(this JObject jObject, string projectName, List<string> locales = null)
    {
        var localesObject = new JObject();

        var root = $"{jObject["projects"][projectName]["root"]}";

        foreach(var locale in locales)
        {
            localesObject.Add(locale, $"{root}/src/locale/messages.{locale}.xlf");
        }

        var projectJObject = jObject["projects"][projectName] as JObject;

        projectJObject.Add("i18n", new JObject
        {
            { "sourceLocale", "en-US" },
            { "locales", localesObject }
        });

        var buildOptions = jObject["projects"][projectName]["architect"]["build"]["options"] as JObject;

        buildOptions.Add("localize", new JArray(locales));
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
