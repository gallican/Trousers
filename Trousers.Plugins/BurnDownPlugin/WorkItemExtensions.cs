using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Plugins.BurnDownPlugin
{
    public static class WorkItemExtensions
    {
        private static readonly string[] _activeStates = new[] {"Active", "Approved", "New", "Committed", "In Progress"};

        public static bool IsComplete(this WorkItemEntity wi)
        {
            return !wi.IsNotComplete();
        }

        public static bool IsNotComplete(this WorkItemEntity wi)
        {
            return _activeStates.Contains(wi.Fields["State"]);
        }
    }
}