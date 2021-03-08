namespace Endpoint.Cli.Services
{
    public interface ITemplateLocator
    {
        string[] Get(string filename);
    }
}