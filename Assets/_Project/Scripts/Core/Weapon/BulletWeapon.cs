using Asteroids.Core.Weapon.Interface;
using Asteroids.Factory.Implementation;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Core.Weapon.Implementation
{
    public class BulletWeapon : IWeapon
    {
        private readonly Unit _unit;
        private readonly CompositeFactory _bulletFactory;
        private readonly Config _config;
        private readonly PlayZone _playZone;
        private DeadZone _deadZone = new();

        private List<CompositeUnit> _bullets = new();

        public BulletWeapon(Unit unit, CompositeFactory bulletFactory, Config config, PlayZone playZone)
        {
            _unit = unit;
            _bulletFactory = bulletFactory;
            _config = config;
            _playZone = playZone;
            RemainingReloadTime = 0;
        }

        public float ReloadTime => _config.ReloadTime;
        public float RemainingReloadTime { get; private set; }

        public void Update(float deltaTime)
        {
            for (var i = _bullets.Count - 1; i >= 0; i--)
            {
                _bullets[i].Update(deltaTime);
                if (_deadZone.CheckInDeadZone(_bullets[i].Unit.Data, _playZone))
                    _bullets[i].Unit.Die(false);
            }
            RemainingReloadTime = Mathf.Max(RemainingReloadTime - deltaTime, 0);
        }

        public bool TryShot()
        {
            if (RemainingReloadTime == 0)
            {
                var bullet = _bulletFactory.Get();
                var data = new TransformData(_unit.Data.Position, Vector2.right.Vector2FromAngle(_unit.Data.Rotation) * _config.BulletSpeed, 0);
                bullet.Unit.Data = data;
                bullet.ForceUpdateTransform();
                _bullets.Add(bullet);
                bullet.Unit.OnDied += OnBulletDead;
                RemainingReloadTime = ReloadTime;
                return true;
            }

            return false;
        }

        public void Clear()
        {
            for (var i = _bullets.Count - 1; i >= 0; i--)
            {
                _bullets[i].Unit.Die(false);
            }
        }

        private void OnBulletDead(Unit unit, bool real)
        {
            var index = _bullets.FindIndex(x => x.Unit == unit);
            _bullets[index].Unit.OnDied -= OnBulletDead;
            _bulletFactory.Release(_bullets[index]);
            _bullets.RemoveAt(index);
        }

        [System.Serializable]
        public struct Config
        {
            public float ReloadTime;
            public float BulletSpeed;
        }
    }
}