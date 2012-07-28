using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Trousers.Core.Infrastructure;

namespace Trousers.Core.DevelopmentStubs
{
    public class WorkItemHistoryProvider : IWorkItemHistoryProvider
    {
        private readonly IFilterExpressionProvider _filterExpressionProvider;
        private IQueryable<WorkItemHistory> _workItemHistories;
        private readonly object _mutex = new object();

        public WorkItemHistoryProvider(IFilterExpressionProvider filterExpressionProvider)
        {
            _filterExpressionProvider = filterExpressionProvider;
            ThreadPool.QueueUserWorkItem(_ => EnsureResults());
        }

        private void EnsureResults()
        {
            if (_workItemHistories != null) return;
            lock (_mutex)
            {
                if (_workItemHistories != null) return;
                _workItemHistories = FetchResults().ToArray().AsQueryable();
            }
        }

        private static IEnumerable<WorkItemHistory> FetchResults()
        {
            yield return new WorkItemHistory { Id = 1, Date = new DateTime(2012, 04, 01), Status = "Active", StoryPoints = 13 };
            yield return new WorkItemHistory { Id = 1, Date = new DateTime(2012, 04, 08), Status = "Active", StoryPoints = 13 };
            yield return new WorkItemHistory { Id = 1, Date = new DateTime(2012, 04, 15), Status = "Active", StoryPoints = 13 };
            yield return new WorkItemHistory { Id = 1, Date = new DateTime(2012, 04, 22), Status = "Resolved", StoryPoints = 13 };
            yield return new WorkItemHistory { Id = 2, Date = new DateTime(2012, 04, 01), Status = "Active", StoryPoints = 5 };
            yield return new WorkItemHistory { Id = 2, Date = new DateTime(2012, 04, 08), Status = "Active", StoryPoints = 5 };
            yield return new WorkItemHistory { Id = 2, Date = new DateTime(2012, 04, 15), Status = "Resolved", StoryPoints = 5 };
            yield return new WorkItemHistory { Id = 2, Date = new DateTime(2012, 04, 22), Status = "Resolved", StoryPoints = 5 };
        }

        public IQueryable<WorkItemHistory> WorkItemHistories
        {
            get
            {
                EnsureResults();
                return _workItemHistories;
            }
        }
    }
}