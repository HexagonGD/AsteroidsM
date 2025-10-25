using Asteroids.Logic.Common.Configs.Implementation;
using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Extensions;
using UnityEngine;

namespace Asteroids.Logic.Common.Movement.Implementation
{
    public partial class AccelerationMovement : IMovement
    {
        private readonly AccelerationMovementConfig _speedData;

        private WASD _input;

        public AccelerationMovement(AccelerationMovementConfig data)
        {
            _input = new WASD();
            _input.Enable();
            _speedData = data;
        }

        public TransformData Update(TransformData data, float deltaTime)
        {
            var inputDirection = _input.Player.Move.ReadValue<Vector2>();
            inputDirection.y = Mathf.Max(inputDirection.y, 0);

            data.Rotation = Mathf.Repeat(data.Rotation - inputDirection.x * _speedData.RotateSpeed * deltaTime, 360f);

            if (inputDirection.y == 0)
                data.Speed = data.Speed.ChangeMagnitude(-_speedData.DecreaseVelocity * deltaTime);
            else
            {
                var direction = Vector2.up.Vector2FromAngle(data.Rotation);
                data.Speed += direction * _speedData.IncreaseVelocity * deltaTime;

                if (data.Speed.magnitude > _speedData.MaxVelocity)
                    data.Speed = data.Speed.SetMagnitude(_speedData.MaxVelocity);
            }

            data += data.Speed * deltaTime;
            return data;
        }
    }
}