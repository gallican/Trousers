using System.Linq;

namespace Trousers.Core.Domain.Queries
{
    public abstract class Query<T>
    {
        public abstract IQueryable<T> Execute(IQueryable<T> source);
    }
}