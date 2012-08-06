using System.Linq;

namespace Trousers.Core.Domain.Queries
{
    public class AllItemsQuery<T> : Query<T>
    {
        public override IQueryable<T> Execute(IQueryable<T> source)
        {
            return source;
        }
    }
}