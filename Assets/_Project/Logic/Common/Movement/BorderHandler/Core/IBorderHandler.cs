using Asteroids.Logic.Common.Movement.Core;

namespace Asteroids.Logic.Common.Movement.BorderHandler.Core
{
    public interface IBorderHandler
    {
        public TransformData Update(TransformData data);
    }
}