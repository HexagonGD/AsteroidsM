using System;

namespace Asteroids.Logic.Common.Configs.Implementation
{
    public class AccelerationMovementConfig : IRemoteConfig
    {
        public float IncreaseVelocity;
        public float DecreaseVelocity;
        public float MaxVelocity;
        public float RotateSpeed;

        public string RemoteName => RemoteNames.ShipMovementConfig;
    }
}