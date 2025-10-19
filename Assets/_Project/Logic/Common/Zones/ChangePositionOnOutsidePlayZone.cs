using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Extensions;
using Asteroids.Logic.Zones;
using UnityEngine;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    public class ChangePositionOnOutsidePlayZone
    {
        private readonly PlayZone _playZone;
        private readonly float _offset;

        public ChangePositionOnOutsidePlayZone(PlayZone playZone, float offset)
        {
            _playZone = playZone;
            _offset = offset;
        }

        public TransformData SetRandomPosition(TransformData data)
        {
            var side = Random.Range(0, 4);
            var position = Vector2.zero;

            switch(side)
            {
                case 0:
                    position.x = _playZone.Width / 2f;
                    position.y = Random.Range(-_playZone.Height / 2f, _playZone.Height / 2f);
                    break;
                case 1:
                    position.x = Random.Range(-_playZone.Width / 2f, _playZone.Width / 2f);
                    position.y = _playZone.Height / 2f;
                    break;
                case 2:
                    position.x = -_playZone.Width / 2f;
                    position.y = Random.Range(-_playZone.Height / 2f, _playZone.Height / 2f);
                    break;
                case 3:
                    position.x = Random.Range(-_playZone.Width / 2f, _playZone.Width / 2f);
                    position.y = -_playZone.Height / 2f;
                    break;
            }

            data.Position = position.ChangeMagnitude(_offset);
            return data;
        }
    }
}