using Asteroids.Core.Movement.Interface;

namespace Asteroids.Core.Movement.Implementation
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