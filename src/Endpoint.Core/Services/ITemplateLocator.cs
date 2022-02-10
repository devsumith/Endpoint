namespace Endpoint.Core.Services
{
    public interface ITemplateLocator
    {
        string[] Get(string filename);
    }
}