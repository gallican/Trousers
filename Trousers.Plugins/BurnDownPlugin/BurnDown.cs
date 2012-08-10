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
        private readonly AverageDeltaCalculator _averageDeltaCalculator;

        public BurnDown(IWorkItemHistoryProvider workItemHistoryProvider, IRepository<WorkItemEntity> repository, AverageDeltaCalculator averageDeltaCalculator)
        {
            _workItemHistoryProvider = workItemHistoryProvider;
            _repository = repository;
            _averageDeltaCalculator = averageDeltaCalculator;
        }

        public Response Query()
        {
            var workItemRevisions = _workItemHistoryProvider.WorkItemHistories.ToList();
            if (workItemRevisions.None()) return new HtmlResponse("No data points. Sorry.");

            Func<DateTime, DateTime> increment = d => d.AddDays(7);

            var earliestDate = workItemRevisions.Min(wi => wi.LastModified).Date.AddDays(1);
            var latestDate = workItemRevisions.Max(wi => wi.LastModified).Date.AddDays(1);
            var totalDays = latestDate.Subtract(earliestDate).TotalDays;
            var projectedDate = latestDate.AddDays(totalDays/4);

            var dataPoints = EnumerableExtensions.Range(earliestDate, latestDate, increment)
                .AsParallel()
                .AsOrdered()
                .Select(date => GenerateActualData(workItemRevisions, date))
                .ToList();

            var projectedWorkAverageDelta = _averageDeltaCalculator.Calculate(dataPoints.Select(dp => (decimal) dp.CumulativeWork));
            var projectedWorkCompletedAverageDelta = _averageDeltaCalculator.Calculate(dataPoints.Select(dp => (decimal) dp.CumulativeWorkCompleted));

            Extrapolate(dataPoints, projectedWorkAverageDelta, projectedWorkCompletedAverageDelta, projectedDate, increment);

            var data = new List<object[]>();
            data.Add(new object[] {"Month", "Completed Work", "Total Work", "Completed Work (est)", "Total Work (est)"});
            data.AddRange(
                dataPoints.Select(dp => new object[] {dp.Date.ToShortDateString(), dp.CumulativeWorkCompleted, dp.CumulativeWork, dp.ProjectedWorkCompleted, dp.ProjectedWork}));
            var options = BuildChartOptions();
            return new ChartResponse(data.ToArray(), options);
        }

        private DataPoint GenerateActualData(IEnumerable<WorkItemEntity> workItemRevisions, DateTime date)
        {
            var allWorkItemsToDate = workItemRevisions.Where(wi => wi.LastModified <= date);
            var latestRevisions = allWorkItemsToDate.LatestRevisions().ToArray();

            var cumulativeWork = CumulativeWork(latestRevisions);
            var cumulativeWorkCompleted = CumulativeWorkCompleted(latestRevisions);

            var dataPoint = new DataPoint {Date = date, CumulativeWorkCompleted = cumulativeWorkCompleted, CumulativeWork = cumulativeWork};
            return dataPoint;
        }

        private static void Extrapolate(ICollection<DataPoint> dataPoints,
                                        decimal projectedWorkAverageDelta,
                                        decimal projectedWorkCompletedAverageDelta,
                                        DateTime projectedDate,
                                        Func<DateTime, DateTime> increment)
        {
            var lastActualDataPoint = dataPoints.OrderByDescending(dp => dp.Date).First();

            var projectionPeriod = 0;
            for (var date = lastActualDataPoint.Date; date <= projectedDate; date = increment(date))
            {
                var projectedWork = lastActualDataPoint.CumulativeWork + (projectedWorkAverageDelta*projectionPeriod);
                var projectedWorkCompleted = lastActualDataPoint.CumulativeWorkCompleted + (projectedWorkCompletedAverageDelta*projectionPeriod);

                var dataPoint = dataPoints
                    .Where(dp => dp.Date == date)
                    .FirstOrDefault();

                if (dataPoint != null)
                {
                    dataPoint.ProjectedWork = projectedWork;
                    dataPoint.ProjectedWorkCompleted = projectedWorkCompleted;
                }
                else
                {
                    dataPoint = new DataPoint {Date = date, ProjectedWork = projectedWork, ProjectedWorkCompleted = projectedWorkCompleted};
                    dataPoints.Add(dataPoint);
                }

                projectionPeriod++;
            }
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
    }
}