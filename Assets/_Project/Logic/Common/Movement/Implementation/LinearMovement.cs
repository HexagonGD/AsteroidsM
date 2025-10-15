using Asteroids.Logic.Common.Movement.Core;

namespace Asteroids.Logic.Common.Movement.Implementation
{
    public class LinearMovement : IMovement
    {
        public TransformData Update(TransformData data, float deltaTime)
        {
            data += data.Speed * deltaTime;
            return data;
        }
    }
}