using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpoint.Core.Models.Artifacts.Projects.Services;

public interface ICoreProjectService
{
    Task DoWorkAsync();

}

