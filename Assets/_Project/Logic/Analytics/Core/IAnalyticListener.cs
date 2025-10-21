using System;
using System.Collections.Generic;

namespace Asteroids.Logic.Analytics.Core
{
    public interface IAnalyticListener
    {
        public event Action<string, IEnumerable<AnalyticParameter>> OnEventListened;
    }
}