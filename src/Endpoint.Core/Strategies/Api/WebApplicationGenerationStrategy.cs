using Endpoint.Core.Models;
using Endpoint.Core.Services;

namespace Endpoint.Core.Strategies.Api
{
    public interface IWebApplicationGenerationStrategy
    {
        string[] Create(MinimalApiProgramModel model);
    }

    public class WebApplicationGenerationStrategy: IWebApplicationGenerationStrategy
    {

        public WebApplicationGenerationStrategy(ITemplateProcessor templateProcessor, ITemplateLocator templateLocator)
        {

        }

        public string[] Create(MinimalApiProgramModel model)
        {
            return new string[0];
        }
    }
}
