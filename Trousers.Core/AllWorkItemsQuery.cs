using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Dtos;

namespace Trousers.Core
{
    public class AllWorkItemsQuery : Query<WorkItemDto>
    {
        public override IEnumerable<WorkItemDto> Execute(IQueryable<WorkItemDto> source)
        {
            return source;
        }
    }
}