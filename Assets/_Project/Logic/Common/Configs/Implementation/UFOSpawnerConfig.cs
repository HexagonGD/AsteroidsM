using Asteroids.Logic.Common.Configs.Core;
using UnityEngine;

namespace Asteroids.Logic.Common.Configs.Implementation
{
    [CreateAssetMenu(fileName = "UFOSpawnerConfig", menuName = "Configs/UFOSpawnerConfig")]
    public class UFOSpawnerConfig : ScriptableObject, IRemoteConfig, ILoopTimerSpawnerConfig
    {
        [field: SerializeField] public float TimeForSpawn { get; private set; }
        [field: SerializeField] public float AccumulatedTime { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        public string RemoteName => RemoteNames.UFOSpawnerConfig;
    }
}