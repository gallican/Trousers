using System.Linq;
using Trousers.Core.Dtos;

namespace Trousers.Core
{
    public interface IWorkItemProvider
    {
        void SetQuery(string expr);
        IQueryable<WorkItemDto> WorkItems { get; }
    }
}