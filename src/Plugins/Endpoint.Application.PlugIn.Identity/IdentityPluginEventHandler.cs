using Endpoint.Application.Core.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Plugin.Identity
{
    public class IdentityPluginEventHandler : INotificationHandler<SolutionTemplateGenerated>
    {
        public Task Handle(SolutionTemplateGenerated notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("works");

            return Task.CompletedTask;
        }
    }

}
