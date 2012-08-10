using System;
using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool None<T>(this IEnumerable<T> items)
        {
            return !items.Any();
        }

        public static bool None<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            return !items.Any(predicate);
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
                yield return item;
            }
        }

        public static void Done<T>(this IEnumerable<T> items)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // just doing this to force an enumeration
            items.LastOrDefault();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        public static IEnumerable<T> Range<T>(T start, T end, Func<T, T> increment) where T : IComparable
        {
            var current = start;
            while (current.CompareTo(end) < 0)
            {
                yield return current;
                current = increment(current);
            }
        }

        public static IEnumerable<WorkItemEntity> LatestRevisions(this IEnumerable<WorkItemEntity> workItems)
        {
            var latestRevisionOfEachWorkItem = workItems
                .GroupBy(wi => wi.Id)
                .Select(g => g.OrderByDescending(w => w.Revision).First())
                .OrderBy(wi => wi.Id)
                .ToArray()
                ;
            return latestRevisionOfEachWorkItem;
        }
    }
}