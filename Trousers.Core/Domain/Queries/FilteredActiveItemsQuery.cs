using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Domain.Queries
{
    public class FilteredActiveItemsQuery : FilterExpressionQuery
    {
        private readonly IDictionary<long, long> _activeWorkItemRevisions;

        public FilteredActiveItemsQuery(string expr, IDictionary<long, long> activeWorkItemRevisions) : base(expr)
        {
            _activeWorkItemRevisions = activeWorkItemRevisions;
        }

        public override IQueryable<WorkItemEntity> Execute(IQueryable<WorkItemEntity> source)
        {
            var active = new AllCurrentItemsQuery<WorkItemEntity>(_activeWorkItemRevisions).Execute(source);
            var filtered = base.Execute(active);
            return filtered;
        }
    }
}