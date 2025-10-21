using Asteroids.Logic.Common.Weapon.Implementation;

namespace Asteroids.Logic.Analytics.Implementation.WeaponListeners
{
    public class BulletWeaponFireAnalyticListener : WeaponFireAnalyticListener<BulletWeapon>
    {
        protected override string EventName => EventNames.BulletWeaponFiredEvent;

        public BulletWeaponFireAnalyticListener(BulletWeapon weapon) : base(weapon)
        {
        }
    }
}