using System.Collections.Generic;

namespace Newtonsoft.Json.Linq;

public static class JObjectExtensions
{
    public static void EnableDefaultStandaloneComponents(this JObject jObject, string projectName)
    {
        jObject["projects"][projectName]["schematics"] = new JObject
        {
            { "@schematics/angular:component", new JObject() {
                { "standalone", true },
                { "style", "scss" }
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

}
