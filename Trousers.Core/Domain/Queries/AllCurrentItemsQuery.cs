using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Domain.Queries
{
    public class AllCurrentItemsQuery<T> : Query<T> where T : IDeactivatable
    {
        public override IQueryable<T> Execute(IQueryable<T> source)
        {
            return source.Where(item => item.IsCurrent);
        }
    }
}