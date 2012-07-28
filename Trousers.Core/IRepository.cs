using System.Collections.Generic;

namespace Trousers.Core
{
    public interface IRepository<T> where T : class, IIdentifiable
    {
        void AddOrUpdate(T item);
        IEnumerable<T> Query(Query<T> query);
    }
}