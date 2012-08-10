using System;

namespace Trousers.Core.Domain.Events
{
    public static class DomainEvents
    {
        private static Func<IEventBroker> _eventBrokerFunc;

        public static void SetEventBrokerFactory(Func<IEventBroker> eventBrokerFunc)
        {
            _eventBrokerFunc = eventBrokerFunc;
        }

        public static void Raise<T>(T domainEvent) where T : IDomainEvent
        {
            _eventBrokerFunc().Raise(domainEvent);
        }
    }
}
