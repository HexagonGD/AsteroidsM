using Asteroids.Logic.Common.Movement.BorderHandler.Core;
using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Services.Factory.Core;
using Asteroids.Logic.Common.Units;

namespace Asteroids.Logic.Common.Services.Factory.Implementation
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