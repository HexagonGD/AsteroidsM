using System;

namespace Asteroids.Logic.Common.Weapon.Core
{
    public interface IWeapon
    {
        public event Action OnFired;

        public void Update(float deltaTime);
        public bool TryShot();
        public void Clear();
    }
}