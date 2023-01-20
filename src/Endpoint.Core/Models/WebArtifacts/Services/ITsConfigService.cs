using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public interface ITsConfigService
{
    Task DoWorkAsync();

}

