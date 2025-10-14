using Asteroids.Core.Weapon.Interface;
using Asteroids.View;
using UnityEngine;

namespace Asteroids.Core.Weapon.Implementation
{
    public class LazerWeapon : IWeapon
    {
        private readonly Unit _unit;
        private readonly LazerRendererController _lazerController;
        private readonly Config _config;

        private RaycastHit2D[] _hits = new RaycastHit2D[25];
        private LayerMask _layerMask;
        private float _accumulatedTime = 0;
        private float _accumulatedDelay = 0;

        public LazerWeapon(Unit unit, LazerRendererController lazerController, Config config)
        {
            _unit = unit;
            _lazerController = lazerController;
            _config = config;

            Charges = Mathf.Min(MaxCharges, _config.StartCharges);
        }

        public int MaxCharges => _config.MaxCharges;
        public int Charges { get; private set; }
        public float RemainingTimeForCharge => _config.ReloadChargeTime - _accumulatedTime;

        public bool TryShot()
        {
            if (Charges > 0 && _accumulatedDelay >= _config.DelayBetweenShots)
            {
                Charges--;
                _accumulatedDelay = 0;

                var hitsCount = Physics2D.RaycastNonAlloc(_unit.Data.Position, Vector2.right.Vector2FromAngle(_unit.Data.Rotation), _hits, 25f, _config.LayerMask);
                for (var i = 0; i < hitsCount && i < _hits.Length; i++)
                {
                    if (_hits[i].collider.TryGetComponent<UnitView>(out var unitView))
                    {
                        unitView.Unit?.Die();
                    }
                }

                _lazerController.DrawLazer(_unit.Data.Position, Vector2.right.Vector2FromAngle(_unit.Data.Rotation));
                return true;
            }

            return false;
        }

        public void Update(float deltaTime)
        {
            _accumulatedDelay += deltaTime;

            if (Charges < MaxCharges)
            {
                _accumulatedTime += deltaTime;
                while (_accumulatedTime >= _config.ReloadChargeTime && Charges < MaxCharges)
                {
                    Charges++;
                    _accumulatedTime -= _config.ReloadChargeTime;
                }
            }

            if (Charges == MaxCharges)
                _accumulatedTime = 0;

            _lazerController.Update(deltaTime);
        }

        public void Clear()
        {
            Charges = Mathf.Min(MaxCharges, _config.StartCharges);
        }

        [System.Serializable]
        public class Config
        {
            [Min(0)] public int MaxCharges;
            [Min(0)] public int StartCharges;
            [Min(0)] public float ReloadChargeTime;
            [Min(0)] public float DelayBetweenShots;
            public LayerMask LayerMask;
        }
    }
}