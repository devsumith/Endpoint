using Newtonsoft.Json.Linq;

namespace Endpoint.Core.Models.WebArtifacts.Extenisons;

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

}
