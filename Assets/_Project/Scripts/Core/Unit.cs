using Asteroids.Core.BorderHandler.Interface;
using Asteroids.Core.Movement.Interface;
using System;

namespace Asteroids.Core
{
    public class Unit
    {
        public event Action<Unit, bool> OnDied;

        public TransformData Data { get; set; }

        private readonly IMovement _movement;
        private IBorderHandler _borderHandler;

        public Unit(IMovement movement, IBorderHandler borderHandler, TransformData data = default)
        {
            Data = data;
            _movement = movement;
            _borderHandler = borderHandler;
        }

        public void Update(float deltaTime)
        {
            Data = _movement.Update(Data, deltaTime);
            Data = _borderHandler.Update(Data);
        }

        public void Die(bool real = true)
        {
            OnDied?.Invoke(this, real);
        }
    }
}