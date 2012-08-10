using System.Collections.Generic;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Events;
using Trousers.Core.Domain.Repositories;
using Trousers.Core.Extensions;

namespace Trousers.Core.Infrastructure
{
    public class CurrentWorkItemsProvider : IHandleEvent<WorkItemsUpdatedEvent>, IHandleEvent<ApplicationStartedEvent>
    {
        private readonly IRepository<WorkItemEntity> _repository;
        private readonly IDictionary<long, long> _activeWorkItemRevisions = new Dictionary<long, long>();

        public CurrentWorkItemsProvider(IRepository<WorkItemEntity> repository)
        {
            _repository = repository;
        }

        public IDictionary<long, long> ActiveWorkItemRevisions
        {
            get { return _activeWorkItemRevisions; }
        }

        public void Handle(WorkItemsUpdatedEvent domainEvent)
        {
            RefreshCurrentWorkItems();
        }

        public void Handle(ApplicationStartedEvent domainEvent)
        {
            RefreshCurrentWorkItems();
        }

        private void RefreshCurrentWorkItems()
        {
            var workItems = _repository.GetAll().LatestRevisions();
            lock (_activeWorkItemRevisions)
            {
                _activeWorkItemRevisions.Clear();
                foreach (var workItem in workItems)
                {
                    _activeWorkItemRevisions[workItem.Id] = workItem.Revision;
                }
            }
        }
    }
}