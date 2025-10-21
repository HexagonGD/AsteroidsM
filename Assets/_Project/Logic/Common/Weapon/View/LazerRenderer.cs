using System;
using UnityEngine;

namespace Asteroids.Logic.Common.Weapon.View
{
    public partial class LazerRenderer
    {
        public event Action<LazerRenderer> EndedDrawEvent;

        private readonly LineRenderer _lineRenderer;
        private readonly LazerRendererConfig _config;

        private float _accumulatedTime = 0;

        public LazerRenderer(LineRenderer lineRenderer, LazerRendererConfig config)
        {
            _lineRenderer = lineRenderer;
            _config = config;
        }

        public void Drawlazer(Vector3 startPosition, Vector3 direction)
        {
            _lineRenderer.SetPosition(0, startPosition);
            _lineRenderer.SetPosition(1, startPosition + direction * _config.Length);

        }

        public void Update(float deltaTime)
        {
            if (_accumulatedTime > _config.LifeTime)
            {
                UnityEngine.Object.Destroy(_lineRenderer.gameObject);
                EndedDrawEvent?.Invoke(this);
                return;
            }

            _accumulatedTime += deltaTime;
            _lineRenderer.startWidth = _config.WidthCurve.Evaluate(_accumulatedTime);
            _lineRenderer.endWidth = _config.WidthCurve.Evaluate(_accumulatedTime);
        }
    }
}