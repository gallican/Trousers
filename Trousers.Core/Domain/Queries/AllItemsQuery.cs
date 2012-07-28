using System.Collections.Generic;
using System.Linq;

namespace Trousers.Core.Domain.Queries
{
    public class AllItemsQuery<T> : Query<T>
    {
        public override IEnumerable<T> Execute(IQueryable<T> source)
        {
            return source;
        }
    }
}