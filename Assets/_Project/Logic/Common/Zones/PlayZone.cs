using UnityEngine;

namespace Asteroids.Logic.Zones
{
    public class PlayZone
    {
        private readonly Camera _camera;

        public float Height => _camera.orthographicSize * 2f;
        public float Width => _camera.orthographicSize * _camera.aspect * 2f;

        public PlayZone(Camera camera)
        {
            _camera = camera;
        }
    }
}