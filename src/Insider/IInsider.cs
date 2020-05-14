using System;
using System.Collections.Generic;

namespace Insider
{
    public interface IInsider
    {
        void SetStates(IEnumerable<(string[] Key, string Value)> states);
        void SetMetrics(IEnumerable<(string[] Key, int Count, TimeSpan Duration)> metrics);
    }
}
