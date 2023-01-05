using MediatR;

namespace Endpoint.Core.Events;

/// <summary>
/// 
/// </summary>
public class SolutionTemplateGenerated : INotification
{
    public string RootDirectory { get; private set; }

    public SolutionTemplateGenerated(string rootDirectory)
    {
        RootDirectory = rootDirectory;
    }
}
