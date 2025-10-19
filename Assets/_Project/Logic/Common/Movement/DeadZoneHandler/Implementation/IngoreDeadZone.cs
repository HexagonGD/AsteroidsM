using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Movement.DeadZoneHandler.Core;

namespace Asteroids.Logic.Common.Movement.DeadZoneHandler.Implementation
{
    public class IngoreDeadZone : IDeadZoneHandler
    {
        public bool InDeadZone(TransformData data) => false;
    }
}