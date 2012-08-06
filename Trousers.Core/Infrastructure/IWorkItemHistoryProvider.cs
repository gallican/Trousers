using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Infrastructure
{
    public interface IWorkItemHistoryProvider
    {
        IQueryable<WorkItemEntity> WorkItemHistories { get; }
    }
}