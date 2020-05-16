﻿using System;

namespace Insider
{
    public interface IInsider
    {
        void SetState(string[] key, string value);
        void SetMetric(string[] key, int count, TimeSpan duration);
    }
}
