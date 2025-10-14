using Asteroids.Core.BorderHandler.Interface;

namespace Asteroids.Core.BorderHandler.Implementation
{
    public class IgnoreBorder : IBorderHandler
    {
        public TransformData Update(TransformData data)
        {
            return data;
        }
    }
}