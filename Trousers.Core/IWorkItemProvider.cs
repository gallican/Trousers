using System.Linq;
using Trousers.Core.Dtos;

namespace Trousers.Core
{
    public interface IWorkItemProvider
    {
        IQueryable<WorkItemDto> WorkItems { get; }
    }
}