using System;
using System.Collections.Generic;

namespace Insider.Local
{
    public class LocalInsider : IInsider
    {
        private readonly ILocalInsiderConfiguration _configuration;

        public LocalInsider(ILocalInsiderConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void SetMetrics(IEnumerable<(string[] Key, int Count, TimeSpan Duration)> metrics)
        {
        }

        public void SetStates(IEnumerable<(string[] Key, string Value)> states)
        {
        }

        public void Run()
        {

        }
    }
}
