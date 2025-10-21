using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Units.Implementation;

namespace Asteroids.Logic.Analytics.Implementation.UnitDiedListeners
{
    public class BigAsteroidDiedAnalyticListener : UnitDiedAnalyticListener<BigAsteroid>
    {
        protected override string EventName => EventNames.BigAsteroidDiedEvent;

        public BigAsteroidDiedAnalyticListener(CompositeUnitRepository unitRepository) : base(unitRepository)
        {
        }
    }
}