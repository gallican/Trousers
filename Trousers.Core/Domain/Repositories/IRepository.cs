using System.Collections.Generic;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Queries;

namespace Trousers.Core.Domain.Repositories
{
    public interface IRepository<T> where T : class, IIdentifiable
    {
        void AddOrUpdate(T item);
        void AddOrUpdate(IEnumerable<T> items);
        IEnumerable<T> Query(Query<T> query);
    }
}