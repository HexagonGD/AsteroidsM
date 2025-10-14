using UnityEngine;

namespace Asteroids.Core
{
    public class DeadZone
    {
        public bool CheckInDeadZone(TransformData data, PlayZone playZone)
        {
            return Mathf.Abs(data.Position.x) > playZone.Width || Mathf.Abs(data.Position.y) > playZone.Height;
        }
    }
}