using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Domain.Queries
{
    public class LatestWorkItemQuery : Query<WorkItemEntity>
    {
        public override IQueryable<WorkItemEntity> Execute(IQueryable<WorkItemEntity> source)
        {
            return source.OrderByDescending(wi => wi.LastModified).Take(1);
        }
    }
}