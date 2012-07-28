using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Trousers.Core;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Events;
using Trousers.Core.Domain.Repositories;
using Trousers.Core.Extensions;

namespace Trousers.Data.Tfs
{
    public class WorkItemRepositoryUpdater : IHandleEvent<WorkItemsUpdatedEvent>
    {
        private readonly IRepository<WorkItemEntity> _repository;

        public WorkItemRepositoryUpdater(IRepository<WorkItemEntity> repository)
        {
            _repository = repository;
        }

        public void Handle(WorkItemsUpdatedEvent domainEvent)
        {
            domainEvent.WorkItems
                .Select(ToDto)
                .Do(dto => _repository.AddOrUpdate(dto))
                .Done();
        }

        private static WorkItemEntity ToDto(WorkItem wi)
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