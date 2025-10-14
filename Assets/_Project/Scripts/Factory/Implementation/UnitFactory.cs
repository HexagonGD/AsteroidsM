using Asteroids.Core;
using Asteroids.Core.BorderHandler.Interface;
using Asteroids.Core.Movement.Interface;
using Asteroids.Factory.Interface;

namespace Asteroids.Factory.Implementation
{
    public class UnitFactory : IFactory<Unit>
    {
        private readonly IMovement _movement;
        private readonly IBorderHandler _borderHandler;

        public UnitFactory(IMovement movement, IBorderHandler borderHandler)
        {
            _movement = movement;
            _borderHandler = borderHandler;
        }

        public Unit Get()
        {
            return new Unit(_movement, _borderHandler);
        }
    }
}