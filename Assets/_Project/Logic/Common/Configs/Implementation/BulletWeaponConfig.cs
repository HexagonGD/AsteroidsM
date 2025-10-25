using System;

namespace Asteroids.Logic.Common.Configs.Implementation
{
    public class BulletWeaponConfig : IRemoteConfig
    {
        public float ReloadTime;
        public float BulletSpeed;

        public string RemoteName => RemoteNames.BulletWeaponConfig;
    }
}