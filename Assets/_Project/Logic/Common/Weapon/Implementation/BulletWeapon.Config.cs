using UnityEngine;

namespace Asteroids.Logic.Common.Weapon.Implementation
{
    public partial class BulletWeapon
    {
        [CreateAssetMenu(fileName = "BulletWeaponConfig", menuName = "Configs/BulletWeaponConfig")]
        public class Config : ScriptableObject
        {
            public float ReloadTime;
            public float BulletSpeed;
        }
    }
}