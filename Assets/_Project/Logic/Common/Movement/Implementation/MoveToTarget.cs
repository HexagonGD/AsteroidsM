using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Units;
using UnityEngine;

namespace Asteroids.Logic.Common.Movement.Implementation
{
    public class MoveToTarget : IMovement
    {
        private readonly Unit _target;
        private readonly Config _congig;

        public MoveToTarget(Unit target, Config config)
        {
            _target = target;
            _congig = config;
        }

        public TransformData Update(TransformData data, float deltaTime)
        {
            var direction = (_target.Data.Position - data.Position).normalized;
            data.Speed = direction * _congig.Speed;
            data.Position += data.Speed * deltaTime;
            return data;
        }

        [System.Serializable]
        public class Config
        {
            [Min(0)] public float Speed;
        }
    }
}