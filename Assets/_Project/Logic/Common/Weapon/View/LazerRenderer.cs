using System;
using UnityEngine;

namespace Asteroids.Logic.Common.Weapon.View
{
    public class LazerRenderer
    {
        public event Action<LazerRenderer> EndedDrawEvent;

        private readonly LineRenderer _lineRenderer;
        private readonly Config _config;

        private float _accumulatedTime = 0;

        public LazerRenderer(LineRenderer lineRenderer, Config config)
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

        [CreateAssetMenu(fileName = "LazerRendererConfig", menuName = "Configs/LazerRendererConfig")]
        public class Config : ScriptableObject
        {
            [field: SerializeField] public float LifeTime { get; private set; }
            [field: SerializeField] public float Length { get; private set; }
            [field: SerializeField] public AnimationCurve WidthCurve { get; private set; }
        }
    }
}