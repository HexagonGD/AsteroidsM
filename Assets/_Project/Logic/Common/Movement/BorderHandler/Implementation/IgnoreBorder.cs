using Asteroids.Logic.Common.Movement.BorderHandler.Core;
using Asteroids.Logic.Common.Movement.Core;

namespace Asteroids.Logic.Common.Movement.BorderHandler.Implementation
{
    public class IgnoreBorder : IBorderHandler
    {
        public TransformData Update(TransformData data)
        {
            return data;
        }
    }
}