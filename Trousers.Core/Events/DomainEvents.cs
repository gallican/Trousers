namespace Trousers.Core.Events
{
    public static class DomainEvents
    {
        private static IEventBroker _eventBroker;

        public static void SetEventBroker(IEventBroker eventBroker)
        {
            _eventBroker = eventBroker;
        }

        public static void Raise<T>(T domainEvent) where T : IDomainEvent
        {
            _eventBroker.Raise(domainEvent);
        }
    }
}
