using System.Linq;

namespace Trousers.Core
{
    public interface IWorkItemHistoryProvider
    {
        IQueryable<WorkItemHistory> WorkItemHistories { get; }
    }
}