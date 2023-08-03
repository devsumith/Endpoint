using Endpoint.Core.Options;

namespace Endpoint.Core.Syntax.Statements;

public class LogStatementModel
{

    public LogStatementModel(RouteType routeType, string resource, SettingsModel settingsModel)
    {
        RouteType = routeType;
        Resource = resource;
        Settings = settingsModel;
    }

    public RouteType RouteType { get; set; }
    public string Resource { get; set; }
    public SettingsModel Settings { get; set; }
}
