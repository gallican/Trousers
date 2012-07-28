using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Trousers.Core.Events;

namespace Trousers.Data.Tfs
{
    public class WorkItemsUpdatedEvent: IDomainEvent
    {
        private readonly WorkItem[] _workItems;

        public WorkItemsUpdatedEvent(WorkItem[] workItems)
        {
            _workItems = workItems;
        }

        public WorkItem[] WorkItems
        {
            get { return _workItems; }
        }
    }
}