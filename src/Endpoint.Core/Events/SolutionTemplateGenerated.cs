using MediatR;

namespace Endpoint.SharedKernal.Events
{
    public class SolutionTemplateGenerated : INotification
    {
        public string RootDirectory { get; private set; }

        public SolutionTemplateGenerated(string rootDirectory)
        {
            RootDirectory = rootDirectory;
        }
    }
}
