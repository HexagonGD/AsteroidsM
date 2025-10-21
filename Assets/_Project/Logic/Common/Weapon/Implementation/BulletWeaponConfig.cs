using UnityEngine;

namespace Asteroids.Logic.Common.Weapon.Implementation
{
    [CreateAssetMenu(fileName = "BulletWeaponConfig", menuName = "Configs/BulletWeaponConfig")]
    public class BulletWeaponConfig : ScriptableObject
    {
        public float ReloadTime;
        public float BulletSpeed;
    }
}