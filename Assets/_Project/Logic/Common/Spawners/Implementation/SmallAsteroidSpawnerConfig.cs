using UnityEngine;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    [CreateAssetMenu(fileName = "SmallAsteroidSpawnerConfig", menuName = "Configs/SmallAsteroidSpawnerConfig")]
    public class SmallAsteroidSpawnerConfig : ScriptableObject
    {
        [field: SerializeField] public int CountChildren { get; private set; }
        [field: SerializeField] public float SpeedCoef { get; private set; }
        [field: SerializeField] public float DeviationDegrees { get; private set; }
    }
}