namespace Asteroids.Logic.Common.Movement.Core
{
    public interface IMovement
    {
        TransformData Update(TransformData data, float deltaTime);
    }
}