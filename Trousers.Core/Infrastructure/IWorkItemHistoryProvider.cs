using System.Linq;

namespace Trousers.Core.Infrastructure
{
    public interface IWorkItemHistoryProvider
    {
        IQueryable<WorkItemHistory> WorkItemHistories { get; }
    }
}