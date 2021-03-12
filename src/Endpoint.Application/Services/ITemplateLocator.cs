namespace Endpoint.Application.Services
{
    public interface ITemplateLocator
    {
        string[] Get(string filename);
    }
}