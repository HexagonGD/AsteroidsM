namespace Asteroids.Core.Weapon.Interface
{
    public interface IWeapon
    {
        public void Update(float deltaTime);
        public bool TryShot();
        public void Clear();
    }
}