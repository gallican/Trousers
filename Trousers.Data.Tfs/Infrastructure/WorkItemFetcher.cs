﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading;
using Autofac;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Events;
using Trousers.Core.Domain.Queries;
using Trousers.Core.Domain.Repositories;
using Trousers.Core.Extensions;
using Trousers.Data.Tfs.Events;

namespace Trousers.Data.Tfs.Infrastructure
{
    public class WorkItemFetcher : IStartable
    {
        private readonly WorkItemStore _workItemStore;
        private readonly IRepository<WorkItemEntity> _repository;

        public WorkItemFetcher(WorkItemStore workItemStore, IRepository<WorkItemEntity> repository )
        {
            _workItemStore = workItemStore;
            _repository = repository;
        }

        public void Start()
        {
            new Thread(FetchWorkItems).Start();
        }

        private void FetchWorkItems()
        {
            var latestWorkItem = _repository.Query(new LatestWorkItemQuery()).FirstOrDefault();

            var latest = latestWorkItem != null
                ? latestWorkItem.LastModified
                : SqlDateTime.MinValue.Value.AddDays(1);

            while (true)
            {
                var workItems = FetchOneBatch(latest);

                var updatedWorkItems = workItems
                    .Where(wi => wi.ChangedDate > latest)
                    .OrderBy(wi => wi.ChangedDate)
                    .ToArray();

                if (updatedWorkItems.None())
                {
                    Thread.Sleep(10 * 60 * 1000);
                    continue;
                }

                latest = updatedWorkItems.Last().ChangedDate;

                DomainEvents.Raise(new WorkItemsFetchedEvent(updatedWorkItems));
            }
        }

        private IEnumerable<WorkItem> FetchOneBatch(DateTime latest)
        {
            var query =
                string.Format(
                    "SELECT Id, [Team Project], Title, State, [Assigned To], [Work Item Type] FROM WorkItems WHERE [Changed Date] >= '{0}' ORDER BY [Changed Date] DESC",
                    new SqlDateTime(latest.Date).ToSqlString());

            var workItems = _workItemStore.Query(query)
                .Cast<WorkItem>()
                .ToArray();

            return workItems;
        }
    }
}