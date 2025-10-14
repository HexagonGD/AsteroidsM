namespace Asteroids.Core.Movement.Interface
{
    public interface IMovement
    {
        TransformData Update(TransformData data, float deltaTime);
    }
}