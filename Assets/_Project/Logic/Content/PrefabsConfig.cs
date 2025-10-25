using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Asteroids.Logic.Content
{
    [CreateAssetMenu(fileName = "PrefabsConfig", menuName = "Configs/PrefabsConfig")]
    public class PrefabsConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReference ShipPrefab;
        [field: SerializeField] public AssetReference UFOPrefab;
        [field: SerializeField] public AssetReference SmallAsteroidPrefab;
        [field: SerializeField] public AssetReference BigAsteroidPrefab;
        [field: SerializeField] public AssetReference BulletPrefab;
        [field: SerializeField] public AssetReference DebugViewPrefab;
        [field: SerializeField] public AssetReference FinalScoreViewPrefab;
        [field: SerializeField] public AssetReference RebirthViewPrefab;
    }
}