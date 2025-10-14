using Asteroids.Core.DieEffects.Interface;
using System;

namespace Asteroids.Core.DieEffects.Implementation
{
    public class InvokeActionDieEffect : IDieEffect
    {
        private readonly Action<Unit> _action;

        public InvokeActionDieEffect(Action<Unit> action)
        {
            _action = action;
        }

        public void Die(Unit unit)
        {
            _action?.Invoke(unit);
        }
    }
}