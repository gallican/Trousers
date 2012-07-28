using Trousers.Core;
using Trousers.Core.Responses;

namespace Trousers.Plugins.BurnDownPlugin
{
    public class BurnDown : IPlugin
    {
        private readonly IWorkItemHistoryProvider _workItemHistoryProvider;

        public BurnDown(IWorkItemHistoryProvider workItemHistoryProvider)
        {
            _workItemHistoryProvider = workItemHistoryProvider;
        }

        public Response Query()
        {
            var data = new[]
            {
                new object[] {"Month", "To Do", "Done", "Overall Story Point Trend", "Done (estimated)"},
                new object[] {"April", 3, 0, 3, 3},
                new object[] {"May", 2, 1, 4, 3},
                new object[] {"June", 2, 2, 5, 4},
                new object[] {"July", null, null, 6, 5},
                new object[] {"August", null, null, 6.5, 6},
                new object[] {"September", null, null, 7, 7},
                new object[] {"October", null, null, 7, 8},
            };

            var options = new
            {
                title = "Demo Chart",
                isStacked = true,
                seriesType = "area",
                series = new object[]
                {
                    new
                    {
                        color = "orange",
                        type = "area",
                    },
                    new
                    {
                        color = "green",
                        type = "area",
                    },
                    new
                    {
                        color = "red",
                        type = "line",
                    },
                    new
                    {
                        color = "green",
                        type = "line",
                    },
                },
            };

            return new ChartResponse(data, options);
        }
    }
}