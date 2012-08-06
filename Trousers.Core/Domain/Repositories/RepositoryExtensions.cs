using System.Linq;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Queries;

namespace Trousers.Core.Domain.Repositories
{
    public static class RepositoryExtensions
    {
        public static IQueryable<T> GetAll<T>(this IRepository<T> repository) where T : class, IIdentifiable, IVersionable
        {
            return repository.Query(new AllItemsQuery<T>());
        }

        public static IQueryable<T> GetAllRevisionsById<T>(this IRepository<T> repository, int id) where T : class, IIdentifiable, IVersionable
        {
            return repository.Query(new AllRevisionsByIdQuery<T>(id));
        }
    }
}