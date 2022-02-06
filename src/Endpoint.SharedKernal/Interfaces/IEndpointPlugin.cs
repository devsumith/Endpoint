using System.Reactive.Subjects;

namespace Endpoint.SharedKernal.Interfaces
{
    public interface IEndpointPlugin
    {
        void Register(IDomainEvents domainEvents);
    }
}
