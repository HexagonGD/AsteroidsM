using Asteroids.Logic.Common.Configs.Core;
using System;

namespace Asteroids.Logic.Common.Configs.Implementation
{
    public class BigAsteroidSpawnerConfig : IRemoteConfig, ILoopTimerSpawnerConfig
    {
        public float TimeForSpawn;
        public float AccumulatedTime;
        public float Speed;


        float ILoopTimerSpawnerConfig.TimeForSpawn => TimeForSpawn;
        float ILoopTimerSpawnerConfig.AccumulatedTime => AccumulatedTime;
        public string RemoteName => RemoteNames.BigAsteroidSpawnerConfig;
    }
}