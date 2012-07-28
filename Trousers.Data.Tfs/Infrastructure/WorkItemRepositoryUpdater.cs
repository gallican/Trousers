using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Events;
using Trousers.Core.Domain.Repositories;
using Trousers.Data.Tfs.Events;

namespace Trousers.Data.Tfs.Infrastructure
{
    public class WorkItemRepositoryUpdater : IHandleEvent<WorkItemsFetchedEvent>
    {
        private readonly IRepository<WorkItemEntity> _repository;

        public WorkItemRepositoryUpdater(IRepository<WorkItemEntity> repository)
        {
            _repository = repository;
        }

        public void Handle(WorkItemsFetchedEvent domainEvent)
        {
            var entities = domainEvent.WorkItems
                .Select(ToEntity);

            _repository.AddOrUpdate(entities);
        }

        private static WorkItemEntity ToEntity(WorkItem wi)
        {
            var fields = new Dictionary<string, string>();

            foreach (var field in wi.Fields.OfType<Field>())
            {
                if (field.Value == null) continue;
                if ((field.Value is int) && (((int) field.Value) == 0)) continue; // don't want zero fields

                var fieldValue = field.Value.ToString();
                if (string.IsNullOrEmpty(fieldValue)) continue; // don't want empty fields

                fields[field.Name] = fieldValue;
            }

            return new WorkItemEntity(wi.Id, wi.ChangedDate, fields);
        }
    }
}