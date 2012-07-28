using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Trousers.Core.Domain.Events;

namespace Trousers.Data.Tfs.Events
{
    public class WorkItemsFetchedEvent: IDomainEvent
    {
        private readonly WorkItem[] _workItems;

        public WorkItemsFetchedEvent(WorkItem[] workItems)
        {
            _workItems = workItems;
        }

        public WorkItem[] WorkItems
        {
            get { return _workItems; }
        }
    }
}