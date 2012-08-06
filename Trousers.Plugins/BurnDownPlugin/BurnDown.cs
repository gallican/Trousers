using System;
using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Repositories;
using Trousers.Core.Extensions;
using Trousers.Core.Infrastructure;
using Trousers.Core.Responses;

namespace Trousers.Plugins.BurnDownPlugin
{
    public class BurnDown : IPlugin
    {
        private static readonly string[] _effortFields = new[]
                                                             {
                                                                 "Story Points",
                                                                 "Effort",
                                                                 "Hours",
                                                                 "Remaining Work",
                                                             };

        private readonly IWorkItemHistoryProvider _workItemHistoryProvider;
        private readonly IRepository<WorkItemEntity> _repository;

        public BurnDown(IWorkItemHistoryProvider workItemHistoryProvider, IRepository<WorkItemEntity> repository)
        {
            _workItemHistoryProvider = workItemHistoryProvider;
            _repository = repository;
        }

        public Response Query()
        {
            var workItemRevisions = _workItemHistoryProvider.WorkItemHistories.ToList();

            var projectedWorkCompletedGradient = ProjectedWorkCompletedGradient(workItemRevisions);
            var projectedWorkGradient = ProjectedWorkGradient(workItemRevisions);

            var earliestDate = workItemRevisions.Min(wi => wi.LastModified).Date.AddDays(1);
            var latestDate = workItemRevisions.Max(wi => wi.LastModified).Date.AddDays(1);
            var totalDays = latestDate.Subtract(earliestDate).TotalDays;
            var projectedDate = latestDate.AddDays(totalDays / 4);

            var dataPoints = EnumerableExtensions.Range(earliestDate, projectedDate, d => d.AddDays(7))
                .AsParallel()
                .AsOrdered()
                .Select(date => GenerateGraphData(workItemRevisions, latestDate, projectedWorkGradient, projectedWorkCompletedGradient, date))
                .ToArray();

            var data = new List<object[]>();
            data.Add(new object[] {"Month", "Completed Work", "Total Work", "Completed Work (est)", "Total Work (est)"});
            data.AddRange(dataPoints);

            var options = BuildChartOptions();

            return new ChartResponse(data.ToArray(), options);
        }

        private decimal ProjectedWorkGradient(List<WorkItemEntity> workItemRevisions)
        {
            //FIXME really hacky way to calculate a gradient. replace with proper line of best fit.  -andrewh 7/8/2012
            var earliestDate = workItemRevisions.Min(wi => wi.LastModified).Date.AddDays(1);
            var latestDate = workItemRevisions.Max(wi => wi.LastModified).Date.AddDays(1);
            var numDays = latestDate.Subtract(earliestDate).TotalDays;
            var numPeriods = (decimal) numDays/7; //FIXME constant
            var halfWay = earliestDate.AddDays(numDays/2);

            var firstHalfWork = CumulativeWork(workItemRevisions.Where(wi => wi.LastModified <= halfWay));
            var totalWork = CumulativeWork(workItemRevisions);
            var gradient = ((totalWork/firstHalfWork) - 1)/numPeriods;

            return gradient;
        }

        private decimal ProjectedWorkCompletedGradient(List<WorkItemEntity> workItemRevisions)
        {
            //FIXME really hacky way to calculate a gradient. replace with proper line of best fit.  -andrewh 7/8/2012
            var earliestDate = workItemRevisions.Min(wi => wi.LastModified).Date.AddDays(1);
            var latestDate = workItemRevisions.Max(wi => wi.LastModified).Date.AddDays(1);
            var numDays = latestDate.Subtract(earliestDate).TotalDays;
            var numPeriods = (decimal) numDays/7; //FIXME constant
            var halfWay = earliestDate.AddDays(numDays/2);

            var firstHalfWorkCompleted = CumulativeWorkCompleted(workItemRevisions.Where(wi => wi.LastModified <= halfWay));
            var totalWorkCompleted = CumulativeWorkCompleted(workItemRevisions);
            var gradient = ((totalWorkCompleted/firstHalfWorkCompleted) - 1)/numPeriods;

            return gradient;
        }

        private static object BuildChartOptions()
        {
            var options = new
                              {
                                  title = "Burn Rates",
                                  isStacked = false,
                                  seriesType = "area",
                                  series = new object[]
                                               {
                                                   new
                                                       {
                                                           color = "green",
                                                           type = "area",
                                                       },
                                                   new
                                                       {
                                                           color = "orange",
                                                           type = "area",
                                                       },
                                                   new
                                                       {
                                                           color = "green",
                                                           type = "line",
                                                       },
                                                   new
                                                       {
                                                           color = "orange",
                                                           type = "line",
                                                       },
                                               },
                              };
            return options;
        }

        private object[] GenerateGraphData(IEnumerable<WorkItemEntity> workItemRevisions,
                                           DateTime latestDate,
                                           decimal projectedWorkGradient,
                                           decimal projectedWorkCompletedGradient,
                                           DateTime date)
        {
            var allWorkItemsToDate = workItemRevisions.Where(wi => wi.LastModified <= date);
            var latestRevisions = LatestRevisions(allWorkItemsToDate).ToArray();

            var cumulativeWork = CumulativeWork(latestRevisions);
            var cumulativeWorkCompleted = CumulativeWorkCompleted(latestRevisions);

            var axisLabel = date.ToShortDateString();
            object[] dataPoint;

            var numPeriodsBeyondLatest = (decimal) date.Subtract(latestDate).TotalDays/7; //FIXME constant
            if (numPeriodsBeyondLatest <= 0)
            {
                dataPoint = new object[] { axisLabel, cumulativeWorkCompleted, cumulativeWork, cumulativeWorkCompleted, cumulativeWork };
            }
            else
            {
                // we're off in projection-land now...
                var projectedWork = cumulativeWork + (cumulativeWork*projectedWorkGradient*numPeriodsBeyondLatest);
                var projectededWorkCompleted = cumulativeWorkCompleted + (cumulativeWorkCompleted*projectedWorkCompletedGradient*numPeriodsBeyondLatest);

                dataPoint = new object[] {axisLabel, null, null, projectededWorkCompleted, projectedWork};
            }

            return dataPoint;
        }

        private decimal CumulativeWork(IEnumerable<WorkItemEntity> latestRevisions)
        {
            var result = TotalOriginalEffort(latestRevisions);
            return result;
        }

        private decimal CumulativeWorkCompleted(IEnumerable<WorkItemEntity> latestRevisions)
        {
            var completedWorkItems = latestRevisions
                .Where(wi => wi.IsComplete())
                .ToArray();

            var result = TotalOriginalEffort(completedWorkItems);
            return result;
        }

        private int TotalOriginalEffort(IEnumerable<WorkItemEntity> userStories)
        {
            var totalEffort = userStories
                .Select(OriginalEffort)
                .Sum();

            return (int) Math.Floor(totalEffort);
        }

        private decimal OriginalEffort(WorkItemEntity wi)
        {
            var revisions = _repository.GetAllRevisionsById(wi.Id)
                .OrderBy(item => item.Revision)
                .ToArray();

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var workItem in revisions)
            {
                var fieldsContainingEffort = workItem.Fields.Where(f => _effortFields.Contains(f.Key)).ToArray();
                if (fieldsContainingEffort.None()) continue;

                var originalEffort = fieldsContainingEffort
                    .Select(f => f.Value)
                    .Select(s => s.ToIntOrDefault(0))
                    .Where(i => i != 0)
                    .FirstOrDefault();

                if (originalEffort != 0) return originalEffort;
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            return 0;
        }

        private static IEnumerable<WorkItemEntity> LatestRevisions(IEnumerable<WorkItemEntity> workItems)
        {
            var lastRevisionOfEachWorkItemThisPeriod = workItems
                .GroupBy(wi => wi.Id)
                .Select(g => g.OrderByDescending(w => w.Revision).First())
                .OrderBy(wi => wi.Id)
                .ToArray()
                ;
            return lastRevisionOfEachWorkItemThisPeriod;
        }
    }
}