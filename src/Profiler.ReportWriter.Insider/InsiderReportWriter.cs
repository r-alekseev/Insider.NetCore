using Insider;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Profiler
{
    public class InsiderReportWriter : IReportWriter
    {
        private readonly IInsider _insider;

        private readonly ConcurrentBag<ISectionMetrics> _metricsBag;
        private readonly IEqualityComparer<string[]> _chainEqualityComparer;

        public InsiderReportWriter(IInsider insider)
        {
            _insider = insider ?? throw new ArgumentNullException(nameof(insider));

            _chainEqualityComparer = new ChainEqualityComparer();
            _metricsBag = new ConcurrentBag<ISectionMetrics>();
        }

        public void Add(ISectionMetrics metrics) => _metricsBag.Add(metrics);

        public void Write()
        {
            var metrics = _metricsBag
                .ToArray()
                .GroupBy(
                    keySelector: m => m.Chain,
                    elementSelector: m => m,
                    resultSelector: (key, list) => (
                        Chain: key,
                        Count: list.Sum(m => m.Count),
                        Duration: TimeSpan.FromTicks(list.Sum(m => m.Elapsed.Ticks))),
                    comparer: _chainEqualityComparer);

            foreach ((string[] chain, int count, TimeSpan duration) in metrics)
            {
                _insider.SetMetric(chain, count, duration);
            }
        }
    }
}
