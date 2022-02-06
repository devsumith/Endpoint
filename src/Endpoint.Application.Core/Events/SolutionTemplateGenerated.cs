using Endpoint.SharedKernal.Models;
using MediatR;

namespace Endpoint.Application.Core.Events
{
    public class SolutionTemplateGenerated: INotification
    {
        public string RootDirectory { get; private set; }

        public SolutionTemplateGenerated(string rootDirectory)
        {
            RootDirectory = rootDirectory;
        }
    }
}
