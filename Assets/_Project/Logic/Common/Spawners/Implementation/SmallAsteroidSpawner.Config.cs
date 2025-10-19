using UnityEngine;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    public partial class SmallAsteroidSpawner
    {
        [CreateAssetMenu(fileName = "SmallAsteroidSpawnerConfig", menuName = "Configs/SmallAsteroidSpawnerConfig")]
        public class Config : ScriptableObject
        {
            [field:SerializeField] public int CountChildren { get; private set; }
            [field: SerializeField] public float SpeedCoef { get; private set; }
            [field: SerializeField] public float DeviationDegrees { get; private set; }
        }
    }
}