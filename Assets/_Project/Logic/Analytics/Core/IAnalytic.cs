using System.Collections.Generic;

namespace Asteroids.Logic.Analytics.Core
{
    public interface IAnalytic
    {
        public void SendEvent(string eventName, IEnumerable<AnalyticParameter> args);
    }
}