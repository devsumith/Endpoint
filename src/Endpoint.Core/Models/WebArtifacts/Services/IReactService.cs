using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public interface IReactService
{
    void Create(ReactAppReferenceModel model);

}

