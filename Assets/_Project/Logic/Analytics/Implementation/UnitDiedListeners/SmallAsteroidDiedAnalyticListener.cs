using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Units.Implementation;

namespace Asteroids.Logic.Analytics.Implementation.UnitDiedListeners
{
    public class SmallAsteroidDiedAnalyticListener : UnitDiedAnalyticListener<SmallAsteroid>
    {
        protected override string EventName => EventNames.SmallAsteroidDiedEvent;

        public SmallAsteroidDiedAnalyticListener(CompositeUnitRepository unitRepository) : base(unitRepository)
        {
        }
    }
}