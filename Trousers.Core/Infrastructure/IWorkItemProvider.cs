using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Infrastructure
{
    public interface IWorkItemProvider
    {
        IQueryable<WorkItemEntity> WorkItems { get; }
    }
}