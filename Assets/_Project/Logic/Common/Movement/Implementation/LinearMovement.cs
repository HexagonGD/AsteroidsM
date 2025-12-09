using Asteroids.Logic.Common.Movement.Core;
using UnityEngine;

namespace Asteroids.Logic.Common.Movement.Implementation
{
    public class LinearMovement : IMovement
    {
        public TransformData Update(TransformData data, float deltaTime)
        {
            data += data.Speed * deltaTime;
            data.Rotation = Vector2.SignedAngle(Vector2.left, data.Speed) + 180;
            return data;
        }
    }
}