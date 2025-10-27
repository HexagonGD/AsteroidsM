using Asteroids.Logic.Common.Configs.Implementation;
using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Services.Factory.Implementation;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Common.Units.Core;
using Asteroids.Logic.Common.Units.Implementation;
using Asteroids.Logic.Common.Weapon.Core;
using Asteroids.Logic.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Logic.Common.Weapon.Implementation
{
    public partial class BulletWeapon : IWeapon, IDisposable
    {
        public event Action OnFired;

        private readonly Unit _unit;
        private readonly CompositeFactory _bulletFactory;
        private readonly BulletWeaponConfig _config;

        private List<CompositeUnit> _bullets = new();

        public BulletWeapon(Ship unit, Unit.Factory bulletFactory, UnitView.Factory unitViewFactory, BulletWeaponConfig config)
        {
            _unit = unit;
            _bulletFactory = new CompositeFactory(bulletFactory, unitViewFactory);
            _config = config;
            RemainingReloadTime = 0;
        }

        public float ReloadTime => _config.ReloadTime;
        public float RemainingReloadTime { get; private set; }

        public void Update(float deltaTime)
        {
            RemainingReloadTime = Mathf.Max(RemainingReloadTime - deltaTime, 0);
            foreach (var bullet in _bullets.ToArray())
                bullet.Update(deltaTime);
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
                OnFired?.Invoke();
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

        public void Dispose()
        {
            Clear();
        }
    }
}