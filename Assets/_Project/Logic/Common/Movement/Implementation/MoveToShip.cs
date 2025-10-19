using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Units.Core;
using Asteroids.Logic.Common.Units.Implementation;

namespace Asteroids.Logic.Common.Movement.Implementation
{
    public class MoveToShip : IMovement
    {
        private readonly Unit _target;

        public MoveToShip(Ship target)
        {
            _target = target;
        }

        public TransformData Update(TransformData data, float deltaTime)
        {
            var direction = (_target.Data.Position - data.Position).normalized;
            data.Speed = direction * data.Speed.magnitude;
            data.Position += data.Speed * deltaTime;
            return data;
        }
    }
}