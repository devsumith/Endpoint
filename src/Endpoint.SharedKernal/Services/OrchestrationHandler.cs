using Endpoint.SharedKernal.Interfaces;
using MediatR;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Endpoint.SharedKernal.Services
{

    public class OrchestrationHandler : IOrchestrationHandler
    {
        private readonly IMediator _mediator;

        public OrchestrationHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Subject<INotification> _messages = new Subject<INotification>();

        public async Task Publish(IEvent @event)
        {
            _messages.OnNext(@event);

            await _mediator.Publish(@event);
        }

        public async Task Publish(IEvent @event, IEvent previousEvent)
        {
            _messages.OnNext(@event);

            await _mediator.Publish(@event);
        }

        public async Task<T> Handle<T>(IEvent startWith, Func<TaskCompletionSource<T>, Action<INotification>> onNextFactory)
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

            _messages.Subscribe(onNextFactory(tcs));

            await Publish(startWith);

            return await tcs.Task;
        }
    }
}
