using UnityEngine;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    public partial class UFOSpawner
    {
        [CreateAssetMenu(fileName = "UFOSpawnerConfig", menuName = "Configs/UFOSpawnerConfig")]
        public class Config : ScriptableObject
        {
            [field: SerializeField] public float TimeForSpawn { get; private set; }
            [field: SerializeField] public float AccumulatedTime { get; private set; }
            [field: SerializeField] public float Speed { get; private set; }
        }
    }
}