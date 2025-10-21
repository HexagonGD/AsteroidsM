using UnityEngine;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    [CreateAssetMenu(fileName = "BigAsteroidSpawnerConfig", menuName = "Configs/BigAsteroidSpawnerConfig")]
    public class BigAsteroidSpawnerConfig : ScriptableObject
    {
        [field: SerializeField] public float TimeForSpawn { get; private set; }
        [field: SerializeField] public float AccumulatedTime { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
    }
}