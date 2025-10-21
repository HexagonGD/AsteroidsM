using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Units.Core;
using System.Collections.Generic;
using System;
using System.Linq;
using Asteroids.Logic.Analytics.Core;

namespace Asteroids.Logic.Analytics.Implementation.UnitDiedListeners
{
    public abstract class UnitDiedAnalyticListener<T> : IAnalyticListener where T : Unit
    {
        public event Action<string, IEnumerable<AnalyticParameter>> OnEventListened;

        private readonly CompositeUnitRepository _unitRepository;

        protected abstract string EventName { get; }

        public UnitDiedAnalyticListener(CompositeUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
            _unitRepository.OnUnitRegistered += UnitRegisteredHandler;
        }

        private void UnitRegisteredHandler(CompositeUnit unit)
        {
            if (unit.Unit is T)
                unit.Unit.OnDied += DiedHandler;
        }

        private void DiedHandler(Unit unit, bool real)
        {
            if (real)
                OnEventListened?.Invoke(EventName, Enumerable.Empty<AnalyticParameter>());
            unit.OnDied -= DiedHandler;
        }
    }
}