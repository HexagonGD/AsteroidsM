using Asteroids.Logic.Common.Weapon.Implementation;

namespace Asteroids.Logic.Analytics.Implementation.WeaponListeners
{
    public class LazerWeaponFireAnalyticListener : WeaponFireAnalyticListener<LazerWeapon>
    {
        protected override string EventName => EventNames.LazerWeaponFiredEvent;

        public LazerWeaponFireAnalyticListener(LazerWeapon weapon) : base(weapon)
        {
        }
    }
}