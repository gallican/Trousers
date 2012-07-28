namespace Trousers.Core.Domain.Events
{
    public interface IEventBroker
    {
        void Raise<T>(T domainEvent) where T: IDomainEvent;
    }
}