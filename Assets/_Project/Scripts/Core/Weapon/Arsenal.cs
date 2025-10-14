using Asteroids.Core.Weapon.Interface;

namespace Asteroids.Core.Weapon
{
    public class Arsenal
    {
        private readonly IWeapon _firstWeapon;
        private readonly IWeapon _secondWeapon;

        private WASD _input;

        public Arsenal(IWeapon firstWeapon, IWeapon secondWeapon)
        {
            _firstWeapon = firstWeapon;
            _secondWeapon = secondWeapon;

            _input = new();
            _input.Enable();
        }

        public void Update(float deltaTime)
        {
            _firstWeapon.Update(deltaTime);
            _secondWeapon.Update(deltaTime);

            if (_input.Player.FirstWeapon.IsPressed())
                _firstWeapon.TryShot();

            if (_input.Player.SecondWeapon.IsPressed())
                _secondWeapon.TryShot();
        }

        public void Clear()
        {
            _firstWeapon.Clear();
            _secondWeapon.Clear();
        }
    }
}