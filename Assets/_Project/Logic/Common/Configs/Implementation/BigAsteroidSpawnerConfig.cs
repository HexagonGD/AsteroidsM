using Asteroids.Logic.Common.Configs.Core;
using UnityEngine;

namespace Asteroids.Logic.Common.Configs.Implementation
{
    [CreateAssetMenu(fileName = "BigAsteroidSpawnerConfig", menuName = "Configs/BigAsteroidSpawnerConfig")]
    public class BigAsteroidSpawnerConfig : ScriptableObject, IRemoteConfig, ILoopTimerSpawnerConfig
    {
        [field: SerializeField] public float TimeForSpawn { get; private set; }
        [field: SerializeField] public float AccumulatedTime { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        public string RemoteName => RemoteNames.BigAsteroidSpawnerConfig;
    }
}