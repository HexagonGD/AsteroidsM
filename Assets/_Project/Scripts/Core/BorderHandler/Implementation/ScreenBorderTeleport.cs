using Asteroids.Core.BorderHandler.Interface;
using UnityEngine;

namespace Asteroids.Core.BorderHandler.Implementation
{
    public class ScreenBorderTeleport : IBorderHandler
    {
        private readonly PlayZone _playZone;

        public ScreenBorderTeleport(PlayZone playZone)
        {
            _playZone = playZone;
        }

        public TransformData Update(TransformData data)
        {
            if (Camera.main == null)
            {
                Debug.LogError("Main camera is null");
                return data;
            }

            data.Position.y = RecalculatePosition(data.Position.y, _playZone.Height / 2f);
            data.Position.x = RecalculatePosition(data.Position.x, _playZone.Width / 2f);

            return data;
        }

        public float RecalculatePosition(float position, float value)
        {
            if (position < -value)
                return position + value * 2f;
            else if (position > value)
                return position - value * 2f;
            else
                return position;
        }
    }
}