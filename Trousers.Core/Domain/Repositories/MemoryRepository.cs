using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Events;
using Trousers.Core.Domain.Queries;

namespace Trousers.Core.Domain.Repositories
{
    public class MemoryRepository<T> : IRepository<T> where T : class, IIdentifiable
    {
        private readonly List<T> _items = new List<T>();

        public void AddOrUpdate(T item)
        {
            AddOrUpdateInternal(item);
            DomainEvents.Raise(new WorkItemsUpdatedEvent());
        }

        public void AddOrUpdate(IEnumerable<T> items)
        {
            foreach (var item in items) AddOrUpdateInternal(item);
            DomainEvents.Raise(new WorkItemsUpdatedEvent());
        }

        public IEnumerable<T> Query(Query<T> query)
        {
            return query.Execute(_items.AsQueryable());
        }

        private void AddOrUpdateInternal(T item)
        {
            var toRemove = _items.Where(t => t.Id == item.Id).FirstOrDefault();
            if (toRemove != null) _items.Remove(toRemove);

            _items.Add(item);
        }
    }
}