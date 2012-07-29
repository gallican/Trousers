using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Domain.Queries
{
    public class FilteredActiveItemsQuery : FilterExpressionQuery
    {
        public FilteredActiveItemsQuery(string expr) : base(expr)
        {
        }

        public override IEnumerable<WorkItemEntity> Execute(IQueryable<WorkItemEntity> source)
        {
            var filtered = base.Execute(source).ToArray();
            var result = filtered.Where(item => item.IsCurrent).ToArray();
            return result;
        }
    }
}