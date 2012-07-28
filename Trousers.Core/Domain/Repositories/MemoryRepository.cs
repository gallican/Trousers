using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Queries;

namespace Trousers.Core.Domain.Repositories
{
    public class MemoryRepository<T> : IRepository<T> where T : class, IIdentifiable
    {
        private readonly List<T> _items = new List<T>();

        public void AddOrUpdate(T item)
        {
            var toRemove = _items.Where(t => t.Id == item.Id).FirstOrDefault();
            if (toRemove != null) _items.Remove(toRemove);

            _items.Add(item);
        }

        public IEnumerable<T> Query(Query<T> query)
        {
            return query.Execute(_items.AsQueryable());
        }
    }
}