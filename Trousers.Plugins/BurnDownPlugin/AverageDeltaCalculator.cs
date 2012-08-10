using System.Collections.Generic;
using System.Linq;

namespace Trousers.Plugins.BurnDownPlugin
{
    public class AverageDeltaCalculator
    {
        public decimal Calculate(IEnumerable<decimal> points)
        {
            var pointsCopy = points.ToArray();

            var pointsWeCareAbout = pointsCopy
                .Skip(pointsCopy.Length/3) // arbitrarily base our estimate only on the most recent third of the data
                .ToArray();

            var delta = pointsWeCareAbout.Last() - pointsWeCareAbout.First();
            var averageDelta = delta/pointsWeCareAbout.Length;
            return averageDelta;
        }
    }
}