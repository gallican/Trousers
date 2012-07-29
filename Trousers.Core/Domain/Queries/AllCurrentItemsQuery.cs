using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Domain.Queries
{
    public class AllCurrentItemsQuery<T> : Query<T> where T : IDeactivatable
    {
        public override IEnumerable<T> Execute(IQueryable<T> source)
        {
            return source.Where(item => item.IsCurrent);
        }
    }
}