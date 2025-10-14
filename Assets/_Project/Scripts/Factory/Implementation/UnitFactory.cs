using Asteroids.Core;
using Asteroids.Core.BorderHandler.Interface;
using Asteroids.Core.DieEffects.Interface;
using Asteroids.Core.Movement.Interface;
using Asteroids.Factory.Interface;

namespace Asteroids.Factory.Implementation
{
    public class UnitFactory : IFactory<Unit>
    {
        private readonly IMovement _movement;
        private readonly IBorderHandler _borderHandler;
        private readonly IDieEffect _dieEffect;

        public UnitFactory(IMovement movement, IBorderHandler borderHandler, IDieEffect dieEffect)
        {
            _movement = movement;
            _borderHandler = borderHandler;
            _dieEffect = dieEffect;
        }

        public Unit Get()
        {
            return new Unit(_movement, _borderHandler, _dieEffect);
        }
    }
}