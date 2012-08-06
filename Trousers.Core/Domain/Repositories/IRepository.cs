using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Queries;

namespace Trousers.Core.Domain.Repositories
{
    public interface IRepository<T> where T : class, IIdentifiable, IVersionable
    {
        void AddOrUpdate(T item);
        void AddOrUpdate(IEnumerable<T> items);
        IQueryable<T> Query(Query<T> query);
    }
}