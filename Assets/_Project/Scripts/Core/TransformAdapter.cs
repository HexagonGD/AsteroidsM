using UnityEngine;

namespace Asteroids.Core
{
    public class TransformAdapter
    {
        public void Update(Transform transform, TransformData data)
        {
            transform.SetLocalPositionAndRotation(data.Position.ToVector3XY(),
                                                  Quaternion.Euler(Vector3.forward * data.Rotation));
        }
    }
}