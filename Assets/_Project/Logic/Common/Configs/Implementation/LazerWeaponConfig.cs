using System;
using UnityEngine;

namespace Asteroids.Logic.Common.Configs.Implementation
{
    public class LazerWeaponConfig : IRemoteConfig
    {
        public int MaxCharges;
        public int StartCharges;
        public float ReloadChargeTime;
        public float DelayBetweenShots;
        public LayerMask LayerMask;

        public string RemoteName => RemoteNames.LazerWeaponConfig;
    }
}