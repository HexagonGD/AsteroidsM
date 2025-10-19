using Asteroids.Logic.Common.UI.Implementation;
using Asteroids.Logic.Common.Units;
using UnityEngine;

namespace Asteroids.Logic.Content
{
    [CreateAssetMenu(fileName = "PrefabsConfig", menuName = "PrefabsConfig")]
    public class PrefabsConfig : ScriptableObject
    {
        [field: SerializeField] public UnitView ShipPrefab;
        [field: SerializeField] public UnitView UFOPrefab;
        [field: SerializeField] public UnitView SmallAsteroidPrefab;
        [field: SerializeField] public UnitView BigAsteroidPrefab;
        [field: SerializeField] public UnitView BulletPrefab;
        [field: SerializeField] public DebugView DebugViewPrefab;
        [field: SerializeField] public FinalScoreView FinalScoreViewPrefab;
    }
}