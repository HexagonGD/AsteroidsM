using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Movement.DeadZoneHandler.Core;
using Asteroids.Logic.Zones;
using UnityEngine;

namespace Asteroids.Logic.Common.Movement.DeadZoneHandler.Implementation
{
    public class OffsetBorderDeadZone : IDeadZoneHandler
    {
        private readonly PlayZone _playZone;
        private readonly float _offset;

        public OffsetBorderDeadZone(PlayZone playZone, float offset)
        {
            _playZone = playZone;
            _offset = offset;
        }


        public bool InDeadZone(TransformData data)
        {
            return Mathf.Abs(data.Position.x) > _playZone.Width / 2 + _offset ||
                   Mathf.Abs(data.Position.y) > _playZone.Height / 2 + _offset;
        }
    }
}