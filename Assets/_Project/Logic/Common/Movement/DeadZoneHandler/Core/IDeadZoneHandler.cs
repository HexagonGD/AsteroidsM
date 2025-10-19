using Asteroids.Logic.Common.Movement.Core;

namespace Asteroids.Logic.Common.Movement.DeadZoneHandler.Core
{
    public interface IDeadZoneHandler
    {
        public bool InDeadZone(TransformData data);
    }
}