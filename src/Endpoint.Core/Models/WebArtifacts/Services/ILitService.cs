using System.Threading.Tasks;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public interface ILitService
{
    Task WorkspaceCreate(string name, string rootDirectory);

}

