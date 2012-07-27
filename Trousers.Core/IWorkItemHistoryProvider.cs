using System.Linq;

namespace Trousers.Core
{
    public interface IWorkItemHistoryProvider
    {
        void SetQuery(string expr);
        IQueryable<WorkItemHistory> WorkItemHistories { get; }
    }
}