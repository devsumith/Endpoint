namespace Endpoint.SharedKernal.Services
{
    public interface ITemplateLocator
    {
        string[] Get(string filename);
    }
}