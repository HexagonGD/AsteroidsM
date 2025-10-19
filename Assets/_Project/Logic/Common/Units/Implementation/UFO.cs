using Asteroids.Logic.Common.DieHandler.Implementation;
using Asteroids.Logic.Common.Movement.BorderHandler.Implementation;
using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Movement.DeadZoneHandler.Implementation;
using Asteroids.Logic.Common.Movement.Implementation;
using Asteroids.Logic.Common.Units.Core;

namespace Asteroids.Logic.Common.Units.Implementation
{
    public class UFO : Unit
    {
        public UFO(MoveToShip movement, IgnoreBorder borderHandler,
                   IngoreDeadZone deadZoneHandler, WithoutDieEffect dieHandler, TransformData data = default) :
                   base(movement, borderHandler, deadZoneHandler, dieHandler, data)
        {
        }
    }
}