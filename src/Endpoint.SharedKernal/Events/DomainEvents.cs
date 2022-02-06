using System;
using System.Reactive.Subjects;

namespace Endpoint.SharedKernal
{
    public interface IDomainEvents
    {
        void Publish(dynamic @event);
        void Subscribe(Action<dynamic> onNext);
    }
    public class DomainEvents: IDomainEvents
    {
        private Subject<dynamic> _messages = new Subject<dynamic>();

        public void Publish(dynamic @event)
        {
            _messages.OnNext(@event);
        }

        public void Subscribe(Action<dynamic> onNext)
        {
            _messages.Subscribe(onNext);
        }
    }
}
