using Asteroids.Logic.Common.DieHandler.Core;
using Asteroids.Logic.Common.Movement.BorderHandler.Core;
using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Movement.DeadZoneHandler.Core;
using System;
using Zenject;

namespace Asteroids.Logic.Common.Units.Core
{
    public abstract class Unit
    {
        public event Action<Unit, bool> OnDied;

        public TransformData Data { get; set; }

        private readonly IMovement _movement;
        private readonly IBorderHandler _borderHandler;
        private readonly IDeadZoneHandler _deadZoneHandler;
        private readonly IDieHandler _dieHandler;

        public Unit(IMovement movement, IBorderHandler borderHandler, IDeadZoneHandler deadZoneHandler,
                    IDieHandler dieHandler, TransformData data = default)
        {
            Data = data;
            _movement = movement;
            _borderHandler = borderHandler;
            _deadZoneHandler = deadZoneHandler;
            _dieHandler = dieHandler;
        }

        public void Update(float deltaTime)
        {
            Data = _movement.Update(Data, deltaTime);
            Data = _borderHandler.Update(Data);
            if (_deadZoneHandler.InDeadZone(Data))
                Die(false);
        }

        public void Die(bool real = true)
        {
            if(real)
                _dieHandler.Handle(this);
            OnDied?.Invoke(this, real);
        }

        public class Factory : PlaceholderFactory<Unit>
        {

        }
    }
}