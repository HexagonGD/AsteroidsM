using Asteroids.Logic.Analytics.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.Logic.Analytics.Implementation.UnitDiedListeners
{
    public class AsteroidDiedAnalyticListener : IAnalyticListener
    {
        public event Action<string, IEnumerable<AnalyticParameter>> OnEventListened;

        private readonly BigAsteroidDiedAnalyticListener _bigAsteroidDiedListener;
        private readonly SmallAsteroidDiedAnalyticListener _smallAsteroidDiedListener;

        public AsteroidDiedAnalyticListener(BigAsteroidDiedAnalyticListener bigAsteroidDiedListener, SmallAsteroidDiedAnalyticListener smallAsteroidDiedListener)
        {
            _bigAsteroidDiedListener = bigAsteroidDiedListener;
            _smallAsteroidDiedListener = smallAsteroidDiedListener;

            _bigAsteroidDiedListener.OnEventListened += AsteroidDiedHandler;
            _smallAsteroidDiedListener.OnEventListened += AsteroidDiedHandler;
        }

        private void AsteroidDiedHandler(string eventName, IEnumerable<AnalyticParameter> parameters)
        {
            OnEventListened?.Invoke(EventNames.AnyAsteroidDiedEvent, Enumerable.Empty<AnalyticParameter>());
        }
    }
}