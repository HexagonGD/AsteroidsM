using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Units.Implementation;

namespace Asteroids.Logic.Analytics.Implementation.UnitDiedListeners
{
    public class UFODiedAnalyticListener : UnitDiedAnalyticListener<UFO>
    {
        protected override string EventName => EventNames.UFODiedEvent;

        public UFODiedAnalyticListener(CompositeUnitRepository unitRepository) : base(unitRepository)
        {
        }
    }
}