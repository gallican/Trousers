using System;

namespace Trousers.Plugins.BurnDownPlugin
{
    public class DataPoint
    {
        public DateTime Date { get; set; }
        public decimal? CumulativeWorkCompleted { get; set; }
        public decimal? CumulativeWork { get; set; }
        public decimal? ProjectedWork { get; set; }
        public decimal? ProjectedWorkCompleted { get; set; }
    }
}