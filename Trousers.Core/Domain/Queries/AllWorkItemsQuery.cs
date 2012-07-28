using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Domain.Queries
{
    public class AllWorkItemsQuery : Query<WorkItemEntity>
    {
        public override IEnumerable<WorkItemEntity> Execute(IQueryable<WorkItemEntity> source)
        {
            return source;
        }
    }
}