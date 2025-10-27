using System;
using System.Collections.Generic;

namespace Asteroids.Logic.Analytics.Core
{
    public class AnalyticManager : IDisposable
    {
        private readonly IAnalytic _analytic;
        private readonly IEnumerable<IAnalyticListener> _listeners;

        public AnalyticManager(IAnalytic analytic, IEnumerable<IAnalyticListener> listeners)
        {
            _analytic = analytic;
            _listeners = listeners;

            foreach (var listener in _listeners)
                listener.OnEventListened += SendEvent;
        }

        private void SendEvent(string eventName, IEnumerable<AnalyticParameter> parameters)
        {
            _analytic.SendEvent(eventName, parameters);
        }

        public void Dispose()
        {
            foreach (var listener in _listeners)
                listener.OnEventListened -= SendEvent;
        }
    }
}