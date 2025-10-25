using UnityEngine;

namespace Asteroids.Logic.Common.Configs.Implementation
{
    [CreateAssetMenu(fileName = "BulletWeaponConfig", menuName = "Configs/BulletWeaponConfig")]
    public class BulletWeaponConfig : ScriptableObject, IRemoteConfig
    {
        public float ReloadTime;
        public float BulletSpeed;

        public string RemoteName => RemoteNames.BulletWeaponConfig;
    }
}