using System.Collections.Generic;
using Autofac;
using Trousers.Core.Domain.Events;

namespace Trousers.Web.Infrastructure
{
    public class AutofacEventBroker : IEventBroker
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacEventBroker(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public void Raise<T>(T domainEvent) where T : IDomainEvent
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var handlers = scope.Resolve<IEnumerable<IHandleEvent<T>>>();
                foreach (var handler in handlers) handler.Handle(domainEvent);
            }
        }
    }
}