using Endpoint.SharedKernal.Models;
using MediatR;

namespace Endpoint.Application.Core.Events
{
    public class SolutionTemplateGenerated: INotification
    {
        public Settings Settings { get; private set; }

        public SolutionTemplateGenerated(Settings settings)
        {
            Settings = settings;
        }
    }
}
