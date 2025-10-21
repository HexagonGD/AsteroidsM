using Asteroids.Logic.Analytics.Core;
using Asteroids.Logic.Common.Weapon.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.Logic.Analytics.Implementation.WeaponListeners
{
    public abstract class WeaponFireAnalyticListener<T> : IAnalyticListener where T : IWeapon
    {
        public event Action<string, IEnumerable<AnalyticParameter>> OnEventListened;

        private readonly T _weapon;

        protected abstract string EventName { get; }

        public WeaponFireAnalyticListener(T weapon)
        {
            _weapon = weapon;
            _weapon.OnFired += FiredHandler;
        }

        private void FiredHandler()
        {
            OnEventListened?.Invoke(EventName, Enumerable.Empty<AnalyticParameter>());
        }
    }
}