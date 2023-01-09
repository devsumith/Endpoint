namespace Endpoint.Core.Models.Artifacts.Files;

public class LaunchSettingsFileModel : FileModel
{
    public LaunchSettingsFileModel(string directory)
        : base("launchSettings", directory, "json")
    { }
}
