using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using Trousers.Plugins.BurnDownPlugin;
using UniMock.Core.BaseTests;

namespace Trousers.UnitTests
{
    [TestFixture]
    public class WhenCalculatingTheAverageDeltaForASimpleSeries : TestFor<AverageDeltaCalculator>
    {
        private IList<decimal> _series;
        private decimal _result;

        protected override void When()
        {
            _series = new decimal[] {0, 1, 2, 3, 4};
            _result = Subject.Calculate(_series);
        }

        [Test]
        public void TheResultShouldBeCorrect()
        {
            _result.ShouldBe(0.75M);
        }
    }
}