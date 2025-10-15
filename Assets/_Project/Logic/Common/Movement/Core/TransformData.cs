using UnityEngine;

namespace Asteroids.Logic.Common.Movement.Core
{
    public struct TransformData
    {
        public Vector2 Position;
        public Vector2 Speed;
        public float Rotation;

        public TransformData(Vector2 position, Vector2 speed, float rotation)
        {
            Position = position;
            Speed = speed;
            Rotation = rotation;
        }

        public static TransformData operator +(TransformData data, Vector2 value)
        {
            data.Position += value;
            return data;
        }
    }
}