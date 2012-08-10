using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Domain.Queries
{
    public class FilteredActiveItemsQuery : FilterExpressionQuery
    {
        public FilteredActiveItemsQuery(string expr) : base(expr)
        {
        }

        public override IQueryable<WorkItemEntity> Execute(IQueryable<WorkItemEntity> source)
        {
            var filtered = base.Execute(source);
            var result = filtered.Where(item => item.IsCurrent);    //FIXME this could be stale.
            return result;
        }
    }
}