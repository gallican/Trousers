namespace Trousers.Core.Domain.Events
{
    public interface IHandleEvent<in T> where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}