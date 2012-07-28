using System.Collections.Generic;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Queries;

namespace Trousers.Core.Domain.Repositories
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<T> GetAll<T>(this IRepository<T> repository) where T : class, IIdentifiable
        {
            return repository.Query(new AllItemsQuery<T>());
        }
    }
}