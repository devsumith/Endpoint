namespace Newtonsoft.Json.Linq;

public static class JObjectExtensions
{
    public static void EnableDefaultStandaloneComponents(this JObject jObject, string projectName)
    {
        var schematicsJObject = jObject["projects"][projectName]["schematics"]["@schematics/angular:component"] as JObject;

        schematicsJObject.Add("standalone", true);
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

}
