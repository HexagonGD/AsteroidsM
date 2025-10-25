using System;

namespace Asteroids.Logic.Common.Configs.Implementation
{
    public class SmallAsteroidSpawnerConfig : IRemoteConfig
    {
        public int CountChildren;
        public float SpeedCoef;
        public float DeviationDegrees;

        public string RemoteName => RemoteNames.SmallAsteroidSpawnerConfig;
    }
}