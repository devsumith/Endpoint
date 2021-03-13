namespace Endpoint.Application.Services
{
    public interface ITenseConverter
    {
        string Convert(string value, bool pastTense = true);
    }
}
